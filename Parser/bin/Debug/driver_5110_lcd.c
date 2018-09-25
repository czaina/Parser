#include "driver_5110_lcd.h"
#include "font.h"
//#include "stm32f10x.h"
#include "lcd.h"
#include <stdio.h>
#include <string.h>


extern void Delay_ms(unsigned long tick);

//Define the LCD Operation function
//void LCD5110_LCD_write_byte(unsigned char dat, unsigned char LCD5110_MOde);

//Define the hardware operation function
void LCD5110_GPIO_Config(void);

void LCD5110_SetFont(bool isBig) 
{
   if(isBig == true)
   {
      // You can obtain a pointer to the array block in a Byte array by taking 
      // the address of the first element and assigning it to a pointer.
      _font.font_ptr = MediumNumbers;
   } 
   
   else if(isBig == false)
   {
      // You can obtain a pointer to the array block in a Byte array by taking 
      // the address of the first element and assigning it to a pointer.
      _font.font_ptr = BigNumbers;
   }
   
    _font.x_size = FONT_READ_BYTE(0);     // 12;
    _font.y_size = FONT_READ_BYTE(1);     // 16;
    _font.offset = FONT_READ_BYTE(2);     // 45;
    _font.numchars = FONT_READ_BYTE(3);   // 13;
    _font.inverted = 0;
   
}




/**
 * Set pin configuration. Doesn't use SPI controller. Just regular pins.
 *
 *	PF11 : Reset
 *	PF7  : DC
 *	PF5  : MISO
 *	PF3  : CLK
 *	PF9  : CE
 *	PF1  : LED control
 *
 * @param None
 * @retval None
 */
void LCD5110_GPIO_Config() {
  
    LCD5110_SetFont(true);
    
}

/**
 * Write character to LCD at current position
 *
 * @param c: char to write
 * @retval None
 */
void LCD5110_write_char(unsigned char c) {
	unsigned char line;
	unsigned char ch = 0;

	c = c - 32;

	for (line = 0; line < 6; line++) {
		ch = font6_8[c][line];
		spi_sendrecv(ch);

	}
}

/**
 * Write character to LCD in inverse video at current location
 *
 * @param c: char to write
 * @retval None
 */
void LCD5110_write_char_inv(unsigned char c) {
	unsigned char line;
	unsigned char ch = 0;

	c = c - 32;

	for (line = 0; line < 6; line++) {
		ch = ~font6_8[c][line];
		spi_sendrecv(ch);

	}
}

/**
 * Write string to LCD at current position. String must be null terminated.
 *
 * @param s: string pointer
 * @retval None
 */
void LCD5110_write_string(char *s) {
	unsigned char ch;
	while (*s != '\0') {
		ch = *s;
		LCD5110_write_char(ch);
		s++;
	}
}



void LCD5110_num_string(char *s, int x, int row) 
{
  unsigned char ch;
  char m = 0;
  while(*s != '\0') {
    ch = *s;
    LCD5110_num_char(ch, x + (m * (_font.x_size)), row);
    s++;
    m++;
  } 
}



/**
 * Write character to LCD at current position
 *
 * @param c: char to write
 * @retval None
 */
void LCD5110_num_char(unsigned char c, int x, int row) 
{
   
//  printf("************* START *************** \n");

    // Caclulate the y-size pixel bank height.
  int max = _font.x_size == 12 ? 2 : 3;
    
    
    int font_idx  = 0;
    
     // look up
     uint16_t index = 0;
        
    for (int rowcnt = 0; rowcnt < max  ; rowcnt++) 
    {
        // Move the y-address pointer for every pointer.
      	lcd_cmd(LCD_SETYADDR | (row + rowcnt)); 
        // Move to the x-address.  
	lcd_cmd(LCD_SETXADDR | x);
        
        // look up font index. 
        
        /*
        
            unsigned char = LOOKUP_TABLE[] = {}
            
            CONFIG = {0x0c, 0x10, 0x2d, 0x0d}
            offset = 45  ( ASCII code for char  "-" )
            x_size = 12
            y_size = 16
        
            x = (ASCII - offset )
            y = x_size *  ( y_size / 5)
            z = x + y + 4; 
        
            index = z;
            
            display char "1" dec "49"
            Example: 
        
            4 = (49 - offset) 
            8.8  =  (12 - (16 / 5))
            16.8 = (4 + 8.8 + 4) 
        
        */
        font_idx = (( c - _font.offset ) * ( _font.x_size * ( _font.y_size / 8 ))) + 4;
        
       // printf("y=> %d, x=> %d,  disp=> %c, font_idx=> %d, rowcnt=> %d \n", row, x, c, font_idx, rowcnt);
        

        for(uint16_t cnt = 0; cnt < _font.x_size ; cnt++)
        {
            index = font_idx + cnt + ( rowcnt * _font.x_size );
            char s = _font.font_ptr[index];
            
         //   printf("cnt=> %d, index => %d, byte => %d \n", cnt, index, s);
            
            spi_sendrecv( s);
        }
    }
    
   // printf("************* END *************** \n");
  
   // reset 
   lcd_cmd(LCD_SETYADDR);
   lcd_cmd(LCD_SETXADDR);
}






void printNumF(double num, char dec, int x, int y, char divider, 
               int length, char filler)
{
	/*char st[27];
	bool neg=false;

	
		if (num<0)
			neg = true;

		atof(st, num, length, dec);

		if (divider != '.')
		{
			for (int i=0; i<sizeof(st); i++)
				if (st[i]=='.')
					st[i]=divider;
		}

		if (filler != ' ')
		{
			if (neg)
			{
				st[0]='-';
				for (int i=1; i<sizeof(st); i++)
					if ((st[i]==' ') || (st[i]=='-'))
						st[i]=filler;
			}
			else
			{
				for (int i=0; i<sizeof(st); i++)
					if (st[i]==' ')
						st[i]=filler;
			}
		}*/

            
		
	

}



void LCD5110_drawBitmap(int x, int y, unsigned char* bitmap, int sx, int sy/*, bool flash*/)
{
	int starty, rows;
    
	starty = y / 8;

	if (sy%8==0)
		rows=sy/8;  
	else
		rows=(sy/8)+1;

	for (int cy=0; cy<rows; cy++)
	{
		lcd_cmd(0x40+(starty+cy));
		lcd_cmd(0x80+x);
		for(int cx=0; cx<sx; cx++)
		{
			//if (flash)
			//LCD5110_LCD_write_byte( bitmap[cx+(cy*sx)], LCD_DATA);
				//else
			spi_sendrecv(bitmap[cx+(cy*sx)]);
		}
	}      
	lcd_cmd(0x40);
	lcd_cmd(0x80);
}


/**
 * Clear display. Write 0 in all memory location.
 *
 * @param None
 * @retval None
 */
void LCD5110_clear() {
	unsigned char i, j;
	for (i = 0; i < 6; i++)
		for (j = 0; j < 84; j++)
			spi_sendrecv(0);
}

/**
 * Set memory current location for characters (set coordinates).
 * Applies only for Fonts with a 6 pixels width.
 *
 * @param X: Column (range from 0 to 13)
 * @param Y: Row (range from 0 to 5)
 * @retval None
 *
 */
void LCD5110_set_XY(unsigned char X, unsigned char Y) {
	unsigned char x;
	x = 6 * X;

	lcd_cmd(0x40 | Y);
	lcd_cmd(0x80 | x);
}

/**
 * Write integer to LCD
 *
 * @param b: integer to write
 * @retval None
 */
void LCD5110_Write_Dec(unsigned int b) {

	/*unsigned char datas[3];

	datas[0] = b / 1000;
	b = b - datas[0] * 1000;
	datas[1] = b / 100;
	b = b - datas[1] * 100;
	datas[2] = b / 10;
	b = b - datas[2] * 10;
	datas[3] = b;

	datas[0] += 48;
	datas[1] += 48;
	datas[2] += 48;
	datas[3] += 48;

	LCD5110_write_char(datas[0]);
	LCD5110_write_char(datas[1]);
	LCD5110_write_char(datas[2]);
	LCD5110_write_char(datas[3]);*/
}
