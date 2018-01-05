using System;
using System.Collections.Generic;
using RDotNet;


namespace RProcesses
{
    public class RUVseq
    {

        private REngine engine;
        private string outputFileDirectory, outputFile, inputFile, rPath;

        public RUVseq(string inputFile, string outputFile, string outputFileDirectory)
        {
            var parse = inputFile.Split('\\');
            this.inputFile = string.Join("\\\\", parse);
            this.outputFile = outputFile;
            parse = outputFileDirectory.Split('\\');
            this.outputFileDirectory = string.Join("\\\\", parse);
            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\R-Core\R");
            rPath = string.Format("{0}{1}",(string)registryKey.GetValue("InstallPath"), "\\bin\\");
            rPath = System.Environment.Is64BitProcess ? string.Format("{0}{1}", rPath, "x64") : string.Format("{0}{1}",rPath, "i386");
            System.Environment.SetEnvironmentVariable("PATH", 
                string.Format("{0}{1}{2}", rPath, System.IO.Path.PathSeparator, System.Environment.GetEnvironmentVariable("PATH")));
            REngine.SetEnvironmentVariables();
            //Console.WriteLine(System.Environment.GetEnvironmentVariable("PATH"));
            parse = rPath.Split('\\');
            rPath = string.Join("\\\\", parse);
        }

        public string run_ruvseq(List<string> group1, List<string> group2)
        {

            try
            {
                engine = REngine.GetInstance();
                engine.Initialize();
                engine.Evaluate(string.Format("Sys.setenv(PATH = paste('{0}', ';', Sys.getenv('PATH'), sep=''))", this.rPath));
                engine.Evaluate("Sys.getenv()");
                engine.Evaluate("library('RUVSeq')");
                engine.Evaluate("library('DESeq2')");
                engine.Evaluate("library('RColorBrewer')");
                engine.Evaluate("matrix <- read.table('" + inputFile + "', header=TRUE, sep=',', fill=TRUE, row.names=1)");
                engine.Evaluate("matrix <- matrix + 1;" +
                    "matrix <- round(matrix, 0);" +
                    "filter <- apply(matrix, 1, function(x) length(x[x > 5]) >= 2);" +
                    "filtered <- matrix[filter,];" +
                    "genes <- rownames(filtered)[grep('^ENS', rownames(filtered))];" +
                    "spikes <- rownames(filtered)[grep('^ERCC', rownames(filtered))];");
                CharacterVector colnames = engine.Evaluate("colnames(filtered)").AsCharacter();
                var x = engine.CreateCharacterVector(this.groupSeparation(colnames, group1, group2));
                engine.SetSymbol("x", x);
                engine.Evaluate("print(x)");
                engine.Evaluate("print(colnames(filtered))");
                engine.Evaluate("colors <- brewer.pal(4, 'Set1');" +
                    "set <- newSeqExpressionSet(as.matrix(filtered), phenoData = data.frame(x, row.names=colnames(filtered)));" +
                    "set <- betweenLaneNormalization(set, which='upper');" +
                    "set1 <- RUVg(set, spikes, k=1);" +
                    "write.csv(normCounts(set1), paste('" + outputFileDirectory + "\\\\" + outputFile + "','_normalizedCounts.csv', sep=''));" +
                    "write.csv(counts(set1), paste('" + outputFileDirectory + "\\\\" + outputFile + "', '_counts.csv', sep = ''));" +
                    "write.csv(pData(set1), paste('" + outputFileDirectory + "\\\\" + outputFile + "','_colData.csv', sep = ''));");
                engine.Evaluate(@"png(file = paste('" + outputFileDirectory + "\\\\" + outputFile + "','_graphRLE_RUV.png', sep = ''), height = 720, width = 1280);" +
                    "plotRLE(set1, outline = FALSE, ylim = c(-4, 4), col = colors[x], xaxt = 'n', ann = FALSE);" +
                    "text(x = seq(1, length(colnames(filtered))) - .2, par('usr')[3] - .4, labels = colnames(filtered), adj = c(1.5, 1.5), srt = 60, pos = 1, xpd = TRUE, cex = 1);" +
                    "axis(1, at = seq(1, length(colnames(filtered)), by = 1), labels = FALSE);" +
                    "dev.off();");
                engine.Evaluate("png(file = paste('" + outputFileDirectory + "\\\\" + outputFile + "', '_graphPCA_RUV.png', sep = '), height = 720, width = 1280);" +
                    "plotPCA(set1, col = colors[x], cex = 1.2);" +
                    "dev.off();");
                return "Success";
            }
            catch (RDotNet.EvaluationException e)
            {
                return e.ToString();
            }
            finally
            {
                engine.Dispose();
            }
        }
        private List<string> groupSeparation(CharacterVector colNames, List<string> group1, List<string> group2)
        {
            List<string> result = new List<string>();
            for(int i = 0; i < group1.Count; ++i)
                group1[i] = group1[i].Replace('-', '.');
            for (int i = 0; i < group2.Count; ++i)
                group2[i] = group2[i].Replace('-', '.');
            foreach (var name in colNames)
            {
                //Console.WriteLine(name);
                if (group1.Contains(name.ToString()))
                {
                    result.Add("G1");
                }
                else
                {
                    result.Add("G2");
                }
            }
            return result;
        }
    }
}

