/* Copyright 2011 Michael Hodgson, Piyabhum Sornpaisarn, Andrew Busse, John Charlesworth, Paramithi Svastisinha

    This file is part of uavcamera.

    uavcamera is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    uavcamera is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with uavcamera.  If not, see <http://www.gnu.org/licenses/>.

*/

/*
 * spi_debug.h
 *
 *  Created on: Nov 21, 2011
 *      Author: mh23g08
 */

#ifndef SPI_DEBUG_H_
#define SPI_DEBUG_H_

#include <stdint.h>
#include <Print.h>

void init_debug_spi();
void send_debug_spi(char* string);

class SPIDebug : public Print
{
  public:
	SPIDebug(int MOSIPin, int CLKPin, int SSPin);
	void begin();
	void write(uint8_t b);
  private:
	int dSPIMOSIPin;
	int dSPISCLKPin;
	int dSPISS;

};



#endif /* SPI_DEBUG_H_ */
