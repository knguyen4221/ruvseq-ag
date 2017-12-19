library(manhattanly)
d <- read.table("S:\\Development\\fastq\\analyses\\graphs\\AST\\counts_differentialExpressionAnalysisWithDESeq2_spikeInNorm.csv",
                header=TRUE, sep = ",", na.rm)
colnames(d) <- c(	"X", "baseMean",	"EFFECTSIZE",	"lfcSE",	"stat",	"pvalue", "P")
volcanoly(d)


