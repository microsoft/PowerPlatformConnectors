
## Cloudmersive PDF
Seamlessly process, generate, modify and permission-control PDFs.

## Pre-requisites
N/A

## API documentation
[Cloudmersive PDF](https://cloudmersive.com/pdf-api) documentation is available on the Cloudmersive website


## Supported Operations
### Encrypt and password-protect a PDF
Encrypt a PDF document with a password.  Set an owner password to control owner (editor/creator) permissions, and set a user (reader) password to control the viewer of the PDF.  Set the password fields null to omit the given password.

### Encrypt, password-protect and set restricted permissions on a PDF
Encrypt a PDF document with a password, and set permissions on the PDF.  Set an owner password to control owner (editor/creator) permissions [required], and set a user (reader) password to control the viewer of the PDF [optional].  Set the reader password to null to omit the password.  Restrict or allow printing, copying content, document assembly, editing (read-only), form filling, modification of annotations, and degraded printing through document Digital Rights Management (DRM).

### Decrypt and password-protect a PDF
Decrypt a PDF document with a password.  Decrypted PDF will no longer require a password to open.

### Add a text watermark to a PDF
Adds a text watermark to a PDF

### Rasterize a PDF to an image-based PDF
Rasterize a PDF into an image-based PDF.  The output is a PDF where each page is comprised of a high-resolution image, with all text, figures and other components removed.

### Remove, delete pages from a PDF document
Remove one or more pages from a PDF document

### Insert, copy pages from one PDF document into another
Copy one or more pages from one PDF document (source document) and insert them into a second PDF document (destination document).

### Get text in a PDF document by page
Gets the text in a PDF by page

### Rotate all pages in a PDF document
Rotate all of the pages in a PDF document by a multiple of 90 degrees

### Rotate a range, subset of pages in a PDF document
Rotate a range of specific pages in a PDF document by a multiple of 90 degrees

### Get PDF document metadata
Returns the metadata from the PDF document, including Title, Author, etc.

### Sets PDF document metadata
Sets (writes) metadata into the input PDF document, including Title, Author, etc.

### Gets PDF Form fields and values
Encrypt a PDF document with a password.  Set an owner password to control owner (editor/creator) permissions, and set a user (reader) password to control the viewer of the PDF.  Set the password fields null to omit the given password.

### Sets ands fills PDF Form field values
Fill in the form fields in a PDF form with specific values.  Use form/get-fields to enumerate the available fields and their data types in an input form.

### Get PDF annotations, including comments in the document
Enumerates the annotations, including comments and notes, in a PDF document.

### Add one or more PDF annotations, comments in the PDF document
Adds one or more annotations, comments to a PDF document.

### Remove a specific PDF annotation, comment in the document
Removes a specific annotation in a PDF document, using the AnnotationIndex.  To enumerate AnnotationIndex for all of the annotations in the PDF document, use the /edit/pdf/annotations/list API.

### Remove all PDF annotations, including comments in the document
Removes all of the annotations, including comments and notes, in a PDF document.



## How to get credentials
- [Register](https://account.cloudmersive.com/signup) for a Cloudmersive Account
- [Sign In](https://account.cloudmersive.com/login) with your Cloudmersive Account and click on API Keys

Here you can create and see your API key(s) listed on the API Keys page.  Simply copy and paste this API Key into the Cloudmersive PDF.

Now you are ready to start using the Cloudmersive PDF.


## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps

