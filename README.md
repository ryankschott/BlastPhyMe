# BlastPhyMe

BLAST, PHYLOGENIES, AND MOLECULAR EVOLUTION

BlastPhyMe: A toolkit for rapid generation and analysis of protein-coding sequence datasets

Ryan K Schott\*1,#, Daniel Gow\*2, Belinda SW Chang1,2
1Department of Ecology and Evolutionary Biology, 2Department of Cell and Systems Biology, University of Toronto, Ontario, Canada
#Current Address: Department of Vertebrate Zoology, National Museum of National History, Smithsonian Institution, Washington DC, USA
*Equal contribution

https://www.biorxiv.org/content/10.1101/059881

BlastPhyMe (BLAST, Phylogenies, and Molecular Evolution) is an application to facilitate the fast and easy generation and analysis of protein-coding sequence datasets. The application uses a portable database framework to manage and organize sequences along with a graphical user interface (GUI) that makes the application extremely easy to use. BlastPhyMe utilizes several existing services and applications in a unique way that save researchers considerable time when building and analyzing protein-coding datasets. The application consists of two modules that can be used separately or together. The first module enables the assembly of coding sequence datasets. BLAST searches can be used to obtain all related sequences of interest from NCBI. Full GenBank records are saved within the database and coding sequences are automatically extracted. A feature of particular note is that sequences can be sorted based on NCBI taxonomic hierarchy before export for visualization using existing tools, such as fast. The application provides GUIs for automatic alignment of sequences with the popular tools MUSCLE and PRANK, as well as for reconstructing phylogenetic trees using PhyML. The second module incorporates selection analyses using codon-based likelihood methods. The alignments and phylogenetic trees generated with the dataset module, or those generated elsewhere, can be used to run the models implemented in the codeml PAML package. A GUI allows easy selection of models and parameters. Importantly, replicate analyses with different parameter starting values can be automatically performed in order to ensure selection of the best-fitting model. Multiple analyses can be run simultaneously based on the number of processor cores available, while additional analyses will be run iteratively until completed. Results are saved within the database and can be exported to publication-ready Excel tables, which further automatically compute the appropriate likelihood ratio test between models in order to determine statistical significance. Future updates will add additional options for phylogenetic reconstruction (eg, MrBayes) and selection analyses (eg, HYPHY). BlastPhyMe saves researches of all bioinformatics experience levels considerable time by automating the numerous tasks required for the generation and analysis of protein-coding sequence datasets using a straightforward graphical interface.

A detailed description of BlastPhyMe as well as a guide for its use can be found in the preprint article: https://www.biorxiv.org/content/10.1101/059881


# Quick Installation Guide

Download the latest full release from: https://github.com/ryankschott/BlastPhyMe/releases.
Extract the folder and run the contained setup.exe file. This will install BlastPhyMe as well as its prerequites. For a full installation guide as well as a list of other programs BlastPhyMe can interact with see the guide here: https://github.com/ryankschott/BlastPhyMe/blob/master/BlastPhyMe%20Installation%20Guide%20-%20v1.5.0.12.pdf
