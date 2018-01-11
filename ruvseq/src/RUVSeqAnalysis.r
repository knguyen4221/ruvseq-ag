if(!require(RUVSeq)){
  source("https://bioconductor.org/biocLite.R")
  biocLite("RUVSeq")
}
if(!require(RColorBrewer)){
  install.packages("RColorBrewer")
}
if(!require(DESeq2)){
  source("https://bioconductor.org/biocLite.R")
  biocLite("DESeq2")
}


matrix <- read.table("C:\\Users\\Ken\\workspace\\ruvseq-ag\\data\\R324R328_output.csv", header=TRUE,sep=",", fill=TRUE, row.names = 1)
matrix <- matrix + 1
matrix <- round(matrix, 0)
filter <- apply(matrix, 1, function(x) length(x[x>5])>=2)
filtered <- matrix[filter,]
genes <- rownames(filtered)[grep("^ENS", rownames(filtered))]
spikes <- rownames(filtered)[grep("^ERCC", rownames(filtered))]

x <- as.factor(c('a','b','c','d'))
set <- newSeqExpressionSet(as.matrix(filtered), phenoData = data.frame(x, row.names=colnames(filtered)))
colors <- brewer.pal(4, "Set1")

# jpeg(file = paste(args[3], "\\", args[4] ,"_graphRLE_filteringAndExploratoryAnalysis.jpeg", sep = ""))
# plotRLE(set, outline=FALSE, ylim=c(-4,4), col=colors[x], xaxt='n', ann=FALSE)
# text(x = seq(1, length(colnames(filtered)))-.2, 
#      par("usr")[3] - .4, labels = colnames(filtered), adj=c(1.5,1.5),
#      srt = 60, pos = 1, xpd=TRUE, cex = 1)
# axis(1, at=seq(1, length(colnames(filtered)), by = 1), labels = FALSE)
# dev.off()
# 
# jpeg(file = paste(args[3], "\\", args[4] ,"_graphPCA_FilteringAndExploratoryAnalysis.jpeg", sep = ""))
# plotPCA(set, col=colors[x], cex=1.2)
# dev.off()
# 
set <- betweenLaneNormalization(set, which="upper")
# par(xaxt="n")
# 
# #jpeg(file = paste(args[3], "\\", args[4] ,"_graphRLE_upperQuartileNorm_FilteringAndExploratoryAnalysis.jpeg", sep = ""))
# plotRLE(set, outline=FALSE, ylim=c(-4,4), col=colors[x], xaxt='n', ann=FALSE)
# text(x = seq(1, length(colnames(filtered)))-.2, 
#      par("usr")[3] - .4, labels = colnames(filtered), adj=c(1.5,1.5),
#      srt = 60, pos = 1, xpd=TRUE, cex = 1)
# axis(1, at=seq(1, length(colnames(filtered)), by = 1), labels = FALSE)
# dev.off()

#jpeg(file = paste(args[3], "\\", args[4] ,"_graphPCA_upperQuartileNorm_FilteringAndExploratoryAnalysis.jpeg", sep =""))
#plotPCA(set, col=colors[x], cex=1.2)
#dev.off()
#Estimating factors of unwanted variation using control genes
set1 <- RUVg(set, spikes, k=1)
#pData(set1)
#spikeInDataFrame <- as.data.frame(pData(set1))
#write.csv(spikeInDataFrame,  paste(args[3],"\\",args[4],"_phenotypicData_spikeInNorm.csv", sep=""))
write.csv(normCounts(set1), paste(args[3],"\\",args[4],"_normalizedCounts_spikeInNorm.csv", sep=""))
write.csv(counts(set1), paste(args[3],"\\",args[4], "_counts_spikeInNorm.csv", sep = ""))
write.csv(pData(set1), paste(args[3], "\\", args[4], "_colData_spikeInNorm.csv", sep = ""))

# write.csv(pData(set1)[Filter(function(x) startsWith(x, "A"), rownames(pData(set1))),], paste(args[3],"\\AST_colData_spikeInNorm.csv", sep=""))
# write.csv(counts(set1)[,Filter(function(x) startsWith(x, "A"), colnames(counts(set1)))], paste(args[3],"\\AST_counts_spikeInNorm.csv", sep=""))
# 
# write.csv(pData(set1)[Filter(function(x) startsWith(x, "B"), rownames(pData(set1))),], paste(args[3],"\\BMEC_colData_spikeInNorm.csv", sep=""))
# write.csv(counts(set1)[,Filter(function(x) startsWith(x, "B"), colnames(counts(set1)))], paste(args[3],"\\BMEC_counts_spikeInNorm.csv", sep=""))
# 
# write.csv(pData(set1)[Filter(function(x) startsWith(x, "N"), rownames(pData(set1))),], paste(args[3],"\\NEU_colData_spikeInNorm.csv", sep=""))
# write.csv(counts(set1)[,Filter(function(x) startsWith(x, "N"), colnames(counts(set1)))], paste(args[3],"\\NEU_counts_spikeInNorm.csv", sep=""))
# 
# write.csv(pData(set1)[Filter(function(x) startsWith(x, "M"), rownames(pData(set1))),], paste(args[3],"\\iMGL_colData_spikeInNorm.csv", sep=""))
# write.csv(counts(set1)[,Filter(function(x) startsWith(x, "M"), colnames(counts(set1)))], paste(args[3],"\\iMGL_counts_spikeInNorm.csv", sep=""))


png(file = paste(args[3], "\\", args[4] ,"_graphRLE_spikeInNorm_RUV.png", sep = ""), height = 720, width = 1280)
plotRLE(set1, outline=FALSE, ylim=c(-4,4), col=colors[x], xaxt='n', ann=FALSE)
text(x = seq(1, length(colnames(filtered)))-.2, 
     par("usr")[3] - .4, labels = colnames(filtered), adj=c(1.5,1.5),
     srt = 60, pos = 1, xpd=TRUE, cex = 1)
axis(1, at=seq(1, length(colnames(filtered)), by = 1), labels = FALSE)
dev.off()

png(file = paste(args[3], "\\", args[4] ,"_graphPCA_spikeInNorm_RUV.png", sep = ""), height = 720, width = 1280)
plotPCA(set1, col=colors[x], cex=1.2)
dev.off()

#Differential Expression Analysis
# design<-model.matrix(~x + W_1, data=pData(set1))
# y <- DGEList(counts=counts(set1), group = x)
# y <- calcNormFactors(y, method="upperquartile")
# y <- estimateGLMCommonDisp(y, design)
# y <- estimateGLMTagwiseDisp(y, design)
# fit <- glmFit(y,design)
# lrt <- glmLRT(fit, coef=2)
# topTags(lrt)

# #Empirical control genes
# design <- model.matrix(~x, data=pData(set))
# y <- DGEList(counts=counts(set), group = x)
# y <- calcNormFactors(y, method="upperquartile")
# y <- estimateGLMCommonDisp(y, design)
# y <- estimateGLMTagwiseDisp(y, design)
# fit <- glmFit(y, design)
# lrt <- glmLRT(fit, coef=2)
# top <- topTags(lrt, n=nrow(set))$table
# empirical <- rownames(set)[which(!(rownames(set) %in% rownames(top)[1:5000]))]
# set2 <- RUVg(set, empirical, k=1)
# pData(set2)
# jpeg(file = paste(args[3], "\\", args[4] ,"graph2-4RLE.jpeg", sep=""))
# plotRLE(set2, outline=FALSE, ylim=c(-4,4), col=colors[x], xaxt='n', ann=FALSE)
# text(x = seq(1, length(colnames(filtered)))-.3, 
#      par("usr")[3] - .5, labels = colnames(filtered), adj=c(1.5,1.5),
#      srt = 60, pos = 1, xpd=TRUE, cex = 1)
# axis(1, at=seq(1, length(colnames(filtered)), by = 1), labels = FALSE)
# dev.off()
# jpeg(file = paste(args[3], "\\", args[4] ,"graph2-4PCA.jpeg", sep = ""))
# plotPCA(set2, col=colors[x], cex=1.2)
# dev.off()

# dds <- DESeqDataSetFromMatrix(countData = counts(set1),
#                               colData = pData(set1),
#                               design = ~W_1 + x)
# dds <- DESeq(dds)
# res <- results(dds, independentFiltering = FALSE)
# res <- as.data.frame(res)
# write.csv(res, paste(args[3],"\\",args[4],"_differentialExpressionAnalysisWithDESeq2_spikeInNorm.csv", sep=""))
# 


par(original.parameters)