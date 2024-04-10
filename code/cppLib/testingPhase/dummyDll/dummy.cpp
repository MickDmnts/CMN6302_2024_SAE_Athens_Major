#include "pch.h"
#include "dummy.h"
#include <vector>
#include <iostream>
#include <fstream>

unsigned int _number;
unsigned char* _externalData = nullptr;
int _size;

unsigned int getNum()
{
	return _number;
}

void setNum(unsigned int num)
{
	_number = num + 1;
}

bool isItCool(bool state)
{
	return !state;
}

void cacheByteArray(unsigned char* externalData, int size)
{
	_externalData = externalData;
	_size = size;

	std::ofstream MyFile("myData.txt");

	for (int i = 0; i < _size; i++)
	{
		MyFile << _externalData[i];
	}

	MyFile.close();
}

//unsigned char* getByteArray()
//{
//	return _externalData;
//}