using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDotNet;

namespace ruvseq.DESeq2
{
    public class DESeq2
    {
        private REngine engine;
        private string colDataFile, countsDataFile, outputPrefix, rPath, outputDir;

        public DESeq2(string colData, string countsData, string outputPrefix, string outputDirectory)
        {
            colDataFile = colData;
            countsDataFile = countsData;
            this.outputPrefix = outputPrefix;
            outputDir = outputDirectory;
            var parse = colDataFile.Split('\\');
            colDataFile = string.Join("\\\\", parse);
            parse = countsDataFile.Split('\\');
            countsDataFile = string.Join("\\\\", parse);
            parse = outputDir.Split('\\');
            outputDir = string.Join("\\\\", parse);

            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\R-Core\R");
            rPath = string.Format("{0}{1}", (string)registryKey.GetValue("InstallPath"), "\\bin\\");
            rPath = System.Environment.Is64BitProcess ? string.Format("{0}{1}", rPath, "x64") : string.Format("{0}{1}", rPath, "i386");
            System.Environment.SetEnvironmentVariable("PATH",
                string.Format("{0}{1}{2}", rPath, System.IO.Path.PathSeparator, System.Environment.GetEnvironmentVariable("PATH")));
            REngine.SetEnvironmentVariables();
            parse = rPath.Split('\\');
            rPath = string.Join("\\\\", parse);
        }

        public string run_deseq2()
        {
            string result = "Success";
            try
            {
                engine = REngine.GetInstance();
                engine.Initialize();
                engine.Evaluate(string.Format("Sys.setenv(PATH = paste('{0}', ';', Sys.getenv('PATH'), sep=''))", this.rPath));
                engine.Evaluate("Sys.getenv()");
                engine.Evaluate("library(\"RUVSeq\"); library(\"DESeq2\"); library(\"RColorBrewer\");");
                engine.Evaluate(string.Format("countsMatrix <- read.table(\"{0}\", header=TRUE,sep=',', fill=TRUE, row.names = 1)", countsDataFile));
                engine.Evaluate(string.Format("colDataMatrix <- read.table(\"{0}\", header=TRUE, sep=',', fill=TRUE, row.names = 1)", colDataFile));
                engine.Evaluate("dds <- DESeqDataSetFromMatrix(countData = countsMatrix, colData = colDataMatrix, design = ~W_1 + x)");
                engine.Evaluate("dds <- DESeq(dds)");
                engine.Evaluate("res <- results(dds, independentFiltering = FALSE)");
                engine.Evaluate("res <- as.data.frame(res)");
                engine.Evaluate(string.Format("write.csv(res, paste(\"{0}\", '\\\\', \"{1}\", '_differentialExpressionAnalysis.csv', sep=''))", outputDir, outputPrefix));
                return result;
            }
            catch(EvaluationException e)
            {
                result = e.ToString();
            }
            finally
            {
                engine.Evaluate("rm(list = ls())");
                engine.Evaluate(@"detach(); detach(); detach();");
            }
            return result;
        }
    }
}
