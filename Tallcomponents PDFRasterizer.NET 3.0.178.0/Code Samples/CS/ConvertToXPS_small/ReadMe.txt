
The problem:
---------------------
Generated XPS files may become big if it contains images.
PDFRasterizer.NET can convert PDF to XPS documents with the help of the .NET frameworks XpsDocumentWriter.
Alas, XpsDocumentWriter seems to insist that all images are stored in a lossless compression format, even
if the original used lossy compression. This may lead to big XPS documents compared to the original PDF 
documents. Even so, this code sample shows a way to create smaller XPS documents.


Lossy compression
---------------------
In normal situations, XpsDocumentWriter writes the images in a XPS as PNG files with lossless compression,
which means that decompression will yield images that are technically equal to the original. This in contrast
to lossy compression like the JPEG files, which yields images which only looks equal. This is done by omitting
the image information which is invisible to the human eye. Usually the lossy compression results in (much) 
smaller file sizes, but may give some visible artifacts at higher compression levels.


The XpsDocumentWriter
---------------------
So XpsDocumentWriter prefers the lossless compression and therefore the XPS documents may get big. This may be
useless if the original image was a lossy JPG file, nothing will be gained this way. TALLRasterizer.NET produces
fixedPages / fixedDocuments which are normally handled directly to the XpsDocumentWriter. At this level there 
are no possibilities to influence the lossy versus lossless behavior. However it appeared to be possible to 
explicitly write lossy JPG files at a lower level. Images can be added to a fixedPage when a new one is created
and then you can choose the type compression you want. Afterwards, it cannot be modified anymore.
So what needs to be done is to break down the fixedPages into its basic elements, down to the level of the 
images, and rebuild new ones with the lossy compression.


This code sample
---------------------
The code consists of 3 steps:
(1) The code first opens the file named "Source.pdf" on the desktop and creates the filenames for the resulting
    big and the small XPS files.
(2) Then the big XPS file is created in the XpsDocumentWriter way, Big because the XpsDocumentWriter seems to
    insists that all images are written in a lossless way.
(3) Finally, the XPS is processed again and lossy image compression is used to make the images smaller. 
    Essentially the PNG files are converted to JPEG files. Next, the references (which includes the "png" 
    extension in the name) must be modified too.
