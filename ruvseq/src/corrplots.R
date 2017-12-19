.libPaths("C:/Users/cnguyen/Documents/R/win-library/3.4")


library(corrplot)
library(RColorBrewer)

makePlot <- function(specific_method) {
args = commandArgs(trailingOnly = TRUE)
count <- read.table(args[1], sep = ",", header= TRUE, fill = TRUE, row.names = 1)
M <- cor(count, method = specific_method, use = "pairwise.complete.obs")

col4 <- colorRampPalette(rev(c("#7F0000","red","#FF7F00","yellow","#7FFF7F", 
                               "cyan", "#007FFF", "blue","#00007F", "purple", "pink", "brown", "#670A0A", "#000000", "#4B0082")))
png(paste(args[2], "\\corrplot_", args[3], "_", "counts_", specific_method, ".png", sep = ""),
     width = 800, height = 800, units = "px")

corrplot(abs(M), order="hclust",type="upper",method="color", col=col4(100),
         cl.lim = c(0,1),outline = T, addgrid.col = "darkgray",tl.cex = 1.0, cl.cex = 1.0, addCoef.col = "white", number.digits = 2, number.cex = 0.80, addshade = c("positive"))
dev.off()

#fpkm <- read.table(args[2], sep = ",", header = TRUE, fill = TRUE, row.names = 1)
#M <- cor(fpkm, method = specific_method, use = "pairwise.complete.obs")
#png(paste(args[3], "\\corrplot_", args[4], "_", "FPKM", ".png", sep = ""),
#     width = 800, height = 800, units = "px")
     
#corrplot(abs(M), order="hclust",type="upper",method="color", col=col4(100),cl.lim = c(0,1),outline = T, addgrid.col = "darkgray",tl.cex = 1.0, cl.cex = 1.0, addCoef.col = "white", number.digits = 2, number.cex = 0.80, addshade = c("positive"))
#dev.off()

}

many_methods <- c("pearson", "spearman")

#if this doesn't work, change back to hardcode
for ( i in many_methods )
  makePlot(i)




