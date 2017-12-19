.libPaths("C:/Users/cnguyen/Documents/R/win-library/3.4")
library(ggplot2)




args = commandArgs(trailingOnly = TRUE)
res <- read.table(args[1], header=TRUE, sep = ",", row.names = 1)




png(paste(args[2],"\\", args[4], "volcanloplot_", args[3], ".png", sep = ""),
    width = 800, height = 800, units = "px")

# Add colored points: highlight if padj<0.05 and if log2FC>2, some color if both)
#res$threshold1 = as.factor(abs(res$log2FoldChange) > 1 & res$pvalue < 0.001)

res$threshold1 = as.factor(
  ifelse(res$pvalue < .001 & res$log2FoldChange > 1, 1, ifelse(res$pvalue < .001 & res$log2FoldChange < 0, -1, 0 )
  )) 

g = ggplot(data=res, aes(x=log2FoldChange, y=-log10(pvalue))) +
  geom_point(aes(colour = threshold1), alpha=0.4) +
  scale_colour_manual(values = c("0" = "gray", "1" = "royalblue", "-1" = "brown1")) +
  xlim(c(-15, 15)) + ylim(c(0, 15)) +
  xlab("log2 fold change") + ylab("-log10 p-value")

g + theme(legend.position = "none", panel.background = element_rect(fill ="white", colour = "black"))

dev.off()