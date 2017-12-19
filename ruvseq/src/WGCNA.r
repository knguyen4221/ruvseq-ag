library('ggplot2')
library('knitr')
library('limma')
library('reshape2')
library('WGCNA')
library('RColorBrewer')
library('gplots')
library('pheatmap')
library('Homo.sapiens')
library('data.table')

counts <- read.table("S:\\Development\\fastq\\analyses\\RUVSeq_output\\counts_counts_spikeInNorm.csv", sep = ",", row.names = 1, header = TRUE)
samples <- colnames(counts)
qq<-t(as.data.frame(strsplit(samples, "_")))
samples <- data.frame(samples, qq[,2], row.names = NULL)
colnames(samples) <- c("celltype", "condition")
samples$condition <- paste("ApoE",samples$condition, sep="")
samples$condition <- as.factor(samples$condition)


#sample check
num_condition <- nlevels(samples$condition)
pal <- colorRampPalette(brewer.pal(num_condition, "Set1"))(num_condition)
cond_colors <- pal[as.integer(samples$condition)]

pheatmap(cor(counts), RowSideColors=cond_colors, trace="none", main='Sample correlations (raw)', fontsize = 8)


#low count filtering
low_count_mask <- rowSums(counts) < ncol(counts)
sprintf("Removing %d low-count genes (%d remaining).", sum(low_count_mask), 
        sum(!low_count_mask))


#log2 transformation

log_counts <- log2(counts+1)
x = melt(as.matrix(log_counts))
colnames(x) <- c('gene_id', 'sample', 'value')
ggplot(x, aes(x = x$value, color = x$sample)) + geom_density() + labs(x = "value", colour = "Samples")

pheatmap(cor(log_counts), 
          trace='none', RowSideColors = cond_colors,main='Sample correlations (log2-transformed)',
          cexRow = .75, cexCol = .75)

#remove non differentially expressed genes
log_counts <- log_counts[apply(log_counts, 1, var) > 0,]
mod <- model.matrix(~0+samples$condition)

colnames(mod) <- levels(samples$condition)

fit <- lmFit(log_counts, design = mod)

condition_pairs <- t(combn(levels(samples$condition), 2))

comparisons <- list()
for (i in 1:nrow(condition_pairs)){
  comparisons[[i]] <- as.character(condition_pairs[i,])
}

sig_genes <- c()

for(conds in comparisons){
  contrast_formula <- paste(conds, collapse=' - ')
  contrast_mat <- makeContrasts(constrasts=contrast_formula, levels=mod)
  contrast_fit <- contrasts.fit(fit, contrast_mat)
  eb <- eBayes(contrast_fit)
  
  sig_genes <- union (sig_genes, rownames(topTable(eb, number=Inf, p.value=0.05)))
}

log_counts <- log_counts[rownames(log_counts) %in% sig_genes,]


#co-expression network construction
cordist <- function(dat){
  cor_matrix <- cor(t(dat))
  dist_matrix <- as.matrix(dist(dat, diag=TRUE, upper=TRUE))
  dist_matrix <- log1p(dist_matrix)
  dist_matrix <- 1 - (dist_matrix / max(dist_matrix))
  
  sign(cor_matrix) * ((abs(cor_matrix) + dist_matrix)/ 2)
}

sim_matrix <- cordist(log_counts)

heatmap_indices <- sample(nrow(sim_matrix))



pheatmap(t(sim_matrix[heatmap_indices, heatmap_indices]),
          trace='none', dendrogram='row',
          main='Similarity matrix',
          density.info='none', revC=TRUE, 
         labels_row = "Gene", labels_col = "Gene", show_colnames = FALSE, show_rownames = FALSE)


#construct adjacency matrix
adj_matrix <- adjacency.fromSimilarity(sim_matrix)
rm(sim_matrix)
gc()

gene_ids <- rownames(adj_matrix)


adj_matrix <- matrix(adj_matrix, nrow=nrow(adj_matrix))
rownames(adj_matrix) <- gene_ids
colnames(adj_matrix) <- gene_ids

pheatmap(t(adj_matrix[heatmap_indices, heatmap_indices]),
          show_rownames = FALSE, show_colnames = FALSE, 
          trace='none', dendrogram='row',
          xlab='Gene', ylab='Gene',
          main='Adjacency matrix',
          density.info='none', revC=TRUE)

#module exporting

net <- blockwiseModules(adj_matrix)

mergedColors = labels2colors(net$colors)

plotDendroAndColors(net$dendrograms[[1]], mergedColors[net$blockGenes[[1]]], "Module colors", dendroLabels = FALSE,
                    hang = 0.03, addGuide=TRUE, guideHang=0.05)

moduleLabels = net$colors
MEs <- net$MEs

geneTree = net$dendrograms[[1]]

sizeGrWindow(12,9)

#pdf(file="eigennet.pdf", wi=9,he=6

par(cex = 1.0)

plotEigengeneNetworks(MEs, "Eigengene dendrogram", marDendro = c(0,4,2,0),plotHeatmaps = FALSE)

plotEigengeneNetworks(MEs, "Eigengene adjacency heatmap", marHeatmap = c(3,4,2,2),plotDendrograms = FALSE, xLabelsAngle = 90)

#dev.off()

for(color in unique(moduleLabels)){
  module=adj_matrix[which(moduleLabels==color)]
  write.table(module, paste("module_", color, ".txt", sep=""), sep="\t", row.names=FALSE, col.names=FALSE, quote=FALSE)
}


# me1 <- adj_matrix[which(moduleLabels==1),]
# 
# #print(me1)
# 
# me1gid <- row.names(me1)
# 
# Me1=print(me1gid)
# 
# write.table(Me1,file="Me1.txt")








