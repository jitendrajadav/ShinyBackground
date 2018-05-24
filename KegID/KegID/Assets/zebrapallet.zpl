^XA
	^FX--the box--^FS
	^FO25,25
		^GB775,1175,4,B,0^FS
	^FX--the kegid text top left--^FS
	^FO50,50
		^AVB,40,40
			^FDKegID^FS
	^FX--the pallet num--^FS
	^FO200,50
		^A0N,56
			^FD@@PALLETBARCODE@@^FS
	^FX--ownername, top right--^FS
	^FO775,110,1
		^A0N,42
			^FD@@OWNERNAME@@^FS
	^FX--owner address--^FS
	^FO775,160,1
		^A0N,32
			^FD@@OWNERADDRESS@@^FS
	^FO775,195,1
		^A0N,32
			^FD@@OWNERCSZ@@^FS
	^FO775,225,1
		^A0N,32
			^FD@@OWNERPHONE@@^FS
	^FX--a line under the owner--^FS
	^FO25,275
		^GB750,1,4,B,1^FS
	^FX--build location^FS
	^FO50,300,0
		^ADN,16
			^FDBUILD LOCATION^FS
	^FO75,325,0
		^A0N,32
			^FD@@STOCKNAME@@^FS
	^FX--batch^FS
	^FO50,375,0
		^ADN,16
			^FDBATCH^FS
	^FO75,400,0
		^A0N,32
			^FD@@BATCHNUM@@^FS
	^FX--build date^FS
	^FO350,375,0
		^ADN,16
			^FDBUILD DATE^FS
	^FO375,400,0
		^A0N,32
			^FD@@BUILDDATE@@^FS
	^FX--the kegs box--^FS
	^FO800,275,1
		^GB200,200,3,B,0^FS
	^FX--the num of kegs^FS
	^FO775,325,1
		^A0N,128
			^FD@@TOTALKEGS@@^FS
	^FX--a line under the header--^FS
	^FO25,475
		^GB750,1,4,B,1^FS
	^FX--summary line 1, box 1--^FS
	^FO25,475
		^GB250,40,2,B,0^FS
	^FX--summary line 1, text 1--^FS
	^FO40,485
		^AFN,18,10
			^FD@@SIZE1@@^FS
	^FX--summary line 1, box 2--^FS
	^FO275,475
		^GB425,40,2,B,0^FS
	^FX--summary line 1, text 2--^FS
	^FO290,485
		^AFN,18,10
			^FD@@CONTENTS1@@^FS
	^FX--summary line 1, box 3--^FS
	^FO800,475,1
		^GB100,40,2,B,0^FS
	^FX--summary line 1, text 3--^FS
	^FO785,485,1
		^AFN,18,10
			^FD@@QTY1@@^FS

	^FX--summary line 2, box 1--^FS
	^FO25,515
		^GB250,40,2,B,0^FS
	^FX--summary line 2, text 1--^FS
	^FO40,525
		^AFN,18,10
			^FD@@SIZE2@@^FS
	^FX--summary line 2, box 2--^FS
	^FO275,515
		^GB425,40,2,B,0^FS
	^FX--summary line 2, text 2--^FS
	^FO290,525
		^AFN,18,10
			^FD@@CONTENTS2@@^FS
	^FX--summary line 2, box 3--^FS
	^FO800,515,1
		^GB100,40,2,B,0^FS
	^FX--summary line 2, text 3--^FS
	^FO785,525,1
		^AFN,18,10
			^FD@@QTY2@@^FS
			
	^FX--summary line 3, box 1--^FS
	^FO25,555
		^GB250,40,2,B,0^FS
	^FX--summary line 3, text 1--^FS
	^FO40,565
		^AFN,18,10
			^FD@@SIZE3@@^FS
	^FX--summary line 3, box 2--^FS
	^FO275,555
		^GB425,40,2,B,0^FS
	^FX--summary line 3, text 2--^FS
	^FO290,565
		^AFN,18,10
			^FD@@CONTENTS3@@^FS
	^FX--summary line 3, box 3--^FS
	^FO800,555,1
		^GB100,40,2,B,0^FS
	^FX--summary line 3, text 3--^FS
	^FO785,565,1
		^AFN,18,10
			^FD@@QTY3@@^FS
			
	^FX--summary line 4, box 1--^FS
	^FO25,595
		^GB250,40,2,B,0^FS
	^FX--summary line 4, text 1--^FS
	^FO40,605
		^AFN,18,10
			^FD@@SIZE4@@^FS
	^FX--summary line 4, box 2--^FS
	^FO275,595
		^GB425,40,2,B,0^FS
	^FX--summary line 4, text 2--^FS
	^FO290,605
		^AFN,18,10
			^FD@@CONTENTS4@@^FS
	^FX--summary line 4, box 3--^FS
	^FO800,595,1
		^GB100,40,2,B,0^FS
	^FX--summary line 4, text 3--^FS
	^FO785,605,1
		^AFN,18,10
			^FD@@QTY4@@^FS
			
	^FX--summary line 5, box 1--^FS
	^FO25,635
		^GB250,40,2,B,0^FS
	^FX--summary line 5, text 1--^FS
	^FO40,645
		^AFN,18,10
			^FD@@SIZE5@@^FS
	^FX--summary line 5, box 2--^FS
	^FO275,635
		^GB425,40,2,B,0^FS
	^FX--summary line 5, text 2--^FS
	^FO290,645
		^AFN,18,10
			^FD@@CONTENTS5@@^FS
	^FX--summary line 5, box 3--^FS
	^FO800,635,1
		^GB100,40,2,B,0^FS
	^FX--summary line 5, text 3--^FS
	^FO785,645,1
		^AFN,18,10
			^FD@@QTY5@@^FS
			
	^FX--summary line 6, box 1--^FS
	^FO25,675
		^GB250,40,2,B,0^FS
	^FX--summary line 6, text 1--^FS
	^FO40,685
		^AFN,18,10
			^FD@@SIZE6@@^FS
	^FX--summary line 6, box 2--^FS
	^FO275,675
		^GB425,40,2,B,0^FS
	^FX--summary line 6, text 2--^FS
	^FO290,685
		^AFN,18,10
			^FD@@CONTENTS6@@^FS
	^FX--summary line 6, box 3--^FS
	^FO800,675,1
		^GB100,40,2,B,0^FS
	^FX--summary line 6, text 3--^FS
	^FO785,685,1
		^AFN,18,10
			^FD@@QTY6@@^FS


	^FX--overflow ~line 6, box --^FS
	^FO25,715
		^GB775,60,2,B,0^FS
	^FX--summary line 7, text 1--^FS
	^FO40,725
		^AFN,16
			^FD@@MORESUMMARY@@^FS
			
	^FX--a line under the summary--^FS
	^FO25,275
		^GB750,1,4,B,1^FS

	^FX--the SSCC label--^FS
	^FO50,780,0
		^ADN,16
			^FDSSCC^FS
	
	
	^FX--prepared for--^FS
	^FO775,780,1
		^ADN,16
			^FDPREPARED FOR^FS
	^FX--a line under the prepared--^FS
	^FO800,805,1
		^GB200,1,2,B,1^FS
	^FX--target location--^FS
	^FO775,820,1
		^A0N,42
			^FD@@TARGETNAME@@^FS
	^FX--owner address--^FS
	^FO775,860,1
		^A0N,32
			^FD@@TARGETADDRESS@@^FS
	^FO775,900,1
		^A0N,32
			^FD@@TARGETCSZ@@^FS
	^FO775,940,1
		^A0N,32
			^FD@@TARGETPHONE@@^FS




	^FX--the QR barcode--^FS
	^FO50,800,0
		^BQN,2,9,Q
			^FDQA,@@PALLETBARCODE@@^FS

	^FX--the code128 barcode--^FS
	^FO50,1050,0^BY2
		^BCN,100,Y,N,N
			^FD@@PALLETBARCODE@@^FS


	^FX--the kegid text bottom right--^FS
	^FO675,1000,0
		^AVB,48,40
			^FDKegID^FS
^XZ