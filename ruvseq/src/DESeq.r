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
args = commandArgs(trailingOnly=TRUE)

original.parameters <- par(no.readonly = TRUE)

countsMatrix <- read.table(args[1], header=TRUE,sep=",", fill=TRUE, row.names = 1)
colDataMatrix <- read.table(args[2], header=TRUE, sep=",", fill=TRUE, row.names = 1)

dds <- DESeqDataSetFromMatrix(countData = countsMatrix,
                              colData = colDataMatrix,
                              design = ~W_1 + x)
dds <- DESeq(dds)
res <- results(dds, independentFiltering = FALSE)
res <- as.data.frame(res)
write.csv(res, paste(args[3],"\\",args[4],"_differentialExpressionAnalysisWithDESeq2_spikeInNorm.csv", sep=""))
