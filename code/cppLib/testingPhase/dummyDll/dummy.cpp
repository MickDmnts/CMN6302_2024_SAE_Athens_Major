#include "pch.h"
#include "dummy.h"
#include <vector>
#include <iostream>
#include <fstream>
#include <filesystem>

static unsigned int _number;
static unsigned char* _externalData = nullptr;
static int _size;

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
	_size = size;
	_externalData = new unsigned char[_size];

	memcpy(_externalData, externalData, _size);
}

unsigned char* getByteArray(int* size)
{
	*size = _size;

	return _externalData;
}