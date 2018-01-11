import os
import re


def add_ercc(data, spikes):
	infile = open(data, 'r')
	lines = infile.readlines()
	infile.close()
	spikeins = open(spikes, 'r')
	for line in range(len(lines)):
		if lines[line].startswith(','):
			ercc=spikeins.readline().strip()
			pattern = re.compile(r'.*gene_id "(.*)".*transcript_id "(.*)"')
			lines[line] = pattern.search(ercc).group(1) + '; ' + pattern.search(ercc).group(2)+ ';' + lines[line]
	outfile = open(data, 'w')
	outfile.writelines(lines)
	outfile.close()
	spikeins.close()


if __name__ == "__main__":
	add_ercc("R324R328_output.csv", "spikeins.txt")
