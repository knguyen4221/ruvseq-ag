using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDotNet;

namespace ruvseq.Volcano
{
    public class VolcanoPlotModel
    {
        private REngine engine;
        private string inputFile, outputDir, rPath, outputPrefix;
        private bool displayNames;
        public VolcanoPlotModel(string inputFile, string outputDir, bool DisplayNames, string prefix)
        {
            if (String.IsNullOrEmpty(outputDir))
                throw new ArgumentException();

            if (!File.Exists(inputFile))
                throw new FileNotFoundException();

            FileAttributes attr = File.GetAttributes(inputFile);
            if(attr.HasFlag(FileAttributes.Directory))
            {
                throw new FileNotFoundException();
            }

            string[] parse;
            this.inputFile = inputFile;
            parse = this.inputFile.Split('\\');
            this.inputFile = string.Join("\\\\", parse);
            this.outputDir = outputDir;
            parse = this.outputDir.Split('\\');
            this.outputDir = string.Join("\\\\", parse);
            this.displayNames = DisplayNames;
            outputPrefix = prefix;

            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\R-Core\R");
            rPath = string.Format("{0}{1}", (string)registryKey.GetValue("InstallPath"), "\\bin\\");
            rPath = System.Environment.Is64BitProcess ? string.Format("{0}{1}", rPath, "x64") : string.Format("{0}{1}", rPath, "i386");
            System.Environment.SetEnvironmentVariable("PATH",
                string.Format("{0}{1}{2}", rPath, System.IO.Path.PathSeparator, System.Environment.GetEnvironmentVariable("PATH")));
            parse = rPath.Split('\\');
            REngine.SetEnvironmentVariables(rPath);
            rPath = string.Join("\\\\", parse);
        }

        public string make_plot()
        {
            string result = "Success";
            try
            {
                engine = REngine.GetInstance();
                engine.Evaluate(string.Format("Sys.setenv(PATH = paste('{0};', Sys.getenv(\"PATH\"), sep=\"\"))", rPath));
                engine.Evaluate("library(ggplot2)");
                engine.Evaluate(string.Format("res <- read.csv(\"{0}\", header=TRUE, row.names=1)", inputFile));
                engine.Evaluate(string.Format("res$threshold <- as.factor(abs(res$log2FoldChange) > 1 & res$pvalue < .001)"));
                engine.Evaluate(@"g <- ggplot(data=res, aes(x=log2FoldChange, y=-log10(pvalue))) + geom_point(aes(colour = threshold), alpha = 0.4) +
  scale_colour_manual(values = c('FALSE' = 'gray', 'TRUE' = 'royalblue')) +
  xlim(c(-10, 10)) + ylim(c(0, 15)) +
  xlab('log2 fold change') + ylab('-log10 p-value')+theme(legend.position = 'none', panel.background = element_rect(fill = 'white', colour = 'black'))");
                if (displayNames)
                    engine.Evaluate("g<-g+geom_text(aes(x=res$log2FoldChange, y=-log10(res$pvalue), label=row.names(res), size=1.2), colour='black')");
                engine.Evaluate(string.Format("ggsave(filename=\"{0}\\\\{1}_volcanoplot.png\")", outputDir, outputPrefix));
            } catch (EvaluationException e)
            {
                result = e.ToString();
            }
            finally
            {
                engine.Evaluate("rm(list = ls())");
                engine.Evaluate(@"detach();");
            }
            return result;
        }

    }
}
