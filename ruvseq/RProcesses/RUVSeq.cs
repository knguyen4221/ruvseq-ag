using System;
using System.Collections.Generic;
using RDotNet;

namespace RProcesses
{
    public class RUVseq
    {
        public RUVseq(string inputFile, List<string> group1, List<string> group2, string outputFile, string outputFileDirectory)
        {
            var parse = inputFile.Split('\\');
            inputFile = string.Join("\\\\", parse);
            parse = outputFileDirectory.Split('\\');
            outputFileDirectory = string.Join("\\\\", parse);
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            try
            {
                engine.Evaluate(
                    @"library('RUVSeq')
library('DESeq2')
library('RColorBrewer')");
                engine.Evaluate("matrix <- read.table('" + inputFile + "', header=TRUE, sep=',', fill=TRUE, row.names=1)");
                engine.Evaluate("matrix <- matrix + 1;" +
                    "matrix <- round(matrix, 0);" +
                    "filter <- apply(matrix, 1, function(x) length(x[x > 5]) >= 2);" +
                    "filtered <- matrix[filter,];" +
                    "genes <- rownames(filtered)[grep('^ENS', rownames(filtered))];" +
                    "spikes <- rownames(filtered)[grep('^ERCC', rownames(filtered))];");
                var x = engine.CreateCharacterVector(group1);
                engine.SetSymbol("x", x);
                engine.Evaluate("colors <- brewer.pal(4, 'Set1');"+
                    "set <- newSeqExpressionSet(as.matrix(filtered), phenoData = data.frame(x, row.names=colnames(filtered)));" +
                    "set <- betweenLaneNormalization(set, which='upper');" +
                    "set1 <- RUVg(set, spikes, k=1);" +
                    "write.csv(normCounts(set1), paste('" + outputFileDirectory + "','" + outputFile + "','_normalizedCounts.csv', sep=''));" +
                    "write.csv(counts(set1), paste('" + outputFileDirectory + "', '" + outputFile + "', '_counts.csv', sep = ''));" +
                    "write.csv(pData(set1), paste('" + outputFileDirectory + "','" + outputFile + "','_colData.csv', sep = ''));");
                engine.Evaluate(@"png(file = paste('" + outputFileDirectory + "','" + outputFile + "','_graphRLE_RUV.png', sep = ''), height = 720, width = 1280);" +
                    "plotRLE(set1, outline = FALSE, ylim = c(-4, 4), col = colors[x], xaxt = 'n', ann = FALSE);" +
                    "text(x = seq(1, length(colnames(filtered))) - .2, par('usr')[3] - .4, labels = colnames(filtered), adj = c(1.5, 1.5), srt = 60, pos = 1, xpd = TRUE, cex = 1);" +
                    "axis(1, at = seq(1, length(colnames(filtered)), by = 1), labels = FALSE);" +
                    "dev.off();");
                engine.Evaluate("png(file = paste('" + outputFileDirectory + "', '" + outputFile + "', '_graphPCA_RUV.png', sep = '), height = 720, width = 1280);" +
                    "plotPCA(set1, col = colors[x], cex = 1.2);" +
                    "dev.off();");
            }
            finally
            {
                engine.Dispose();
            }
        }
    }
}

