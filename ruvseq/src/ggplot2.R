library(ggplot2)
res <- read.table("S:\\Development\\fastq\\analyses\\graphs\\AST\\counts_differentialExpressionAnalysisWithDESeq2_spikeInNorm.csv",
                  header=TRUE, sep = ",", row.names = 1)




# Add colored points: highlight if padj<0.05 and if log2FC>2, some color if both)
res$threshold1 = as.factor(abs(res$log2FoldChange) > 1 & res$pvalue < 0.001)

g = ggplot(data=res, aes(x=log2FoldChange, y=-log10(pvalue))) +
  geom_point(aes(colour = threshold1), alpha=0.4) +
  scale_colour_manual(values = c("FALSE" = "gray", "TRUE" = "royalblue")) +
  xlim(c(-10, 10)) + ylim(c(0, 15)) +
  xlab("log2 fold change") + ylab("-log10 p-value")

g + theme(legend.position = "none", panel.background = element_rect(fill ="white", colour = "black")) 
# add names to the ggplot 
#+ geom_text(aes(x=res$log2FoldChange, y=-log10(res$pvalue),
 #                                                   label=row.names(res), size=1.2), colour="black")
