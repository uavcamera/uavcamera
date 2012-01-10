This folder contains the schematics and PCB layouts for the uavcamera project, 
as well as some other related files.

The uavcamera team recommends Spirit Circuits (http://www.spiritcircuits.com/) 
for PCB manufacture. They will accept the Gerber files in the gerbers/ folder, 
and have excellent customer service. payload_644.pcb was prototyped with their 
"Go Naked" service, and works like a charm.

### 

All *.sch files were produced using gschem 1.6.2.20110115 from the gEDA 
toolsuite.

All *.pcb files were produced using PCB version 20110918 from the gEDA 
toolsuite.

It is recommended that you run gschem/pcb in this folder to include all of 
the symbols and footprints contained within components/

All other files within this folder were either generated using the export 
tools in gschem/pcb, or in the case of PDFs, ps2pdf.

### Files:

**dummy_payload.* :** Files relating to the dummy payload. This is modified from 
the schematic provided by our customer in sample_peripheral/schematic.pdf, 
and created to test sending data from the autopilot module to the ground 
station. This has only existed on breadboard.

**payload_644.* :** Files relating to the final, implemented and delivered 
payload. This exists on PCB

**payload_smt.* :** This is very similar to payload_644.*, except it uses surface 
mount components where possible and therefore is approximately half the size. 
This has not been tested.

**payload_arduino_mega168.* :** Files relating to a possible payload 
implementation involving an Arduino Uno communicating with the camera, with 
a separate ATmega168 on a daughter board transmitting data to the autopilot. 
This relates to the daughter board only, and has not been implemented.

**payload_arduino_muxed.* :** Files relating to a possible payload implementation 
involving an Arduino Uno, with its UART channel multiplexed to communicate 
with both the camera and the autopilot. This relates to a daughter board only, 
and has not been implemented.

### Folder Structure:

**. :** This contains all the schematic and PCB layout files. gafrc tells 
gschem where to find the non-standard symbols, and projectrc tells pcb where 
to find non-standard footprints when running gsch2pcb. basket_* are .csv 
files exported from Farnell, containing all the components featured on the 
PCBs.

**components :** Contains all the non-standard symbols and footprints. These 
are mostly sourced from http://www.gedasymbols.org, and are GPL'd.

**gerbers :** Exported from pcb, these Gerber/RS-274X files can be sent 
straight to a PCB manufacturer.

**images :** Exported from gschem/pcb, these images allow someone to view the 
schematics/PCB designs without requiring the aforementioned tools.

- GDP Group 18
