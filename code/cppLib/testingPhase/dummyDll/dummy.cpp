#include "pch.h"
#include "dummy.h"
#include <vector>
#include <iostream>
#include <fstream>
#include <filesystem>

static int _number;
static unsigned char* _externalData = nullptr;
static int _size;

int getNum()
{
	return _number;
}

void setNum(int num)
{
	_number = num + 1;
}

bool isItCool(bool state)
{
	return !state;
}

int cacheByteArray(unsigned char* externalData, int size)
{
	int result = -1;

	try
	{
		_size = size;
		_externalData = new unsigned char[_size];

		memcpy(_externalData, externalData, _size);

		result = 0;
	}
	catch (...)
	{
		result = 1;
	}

	return result;
}

unsigned char* getByteArray(int* size)
{
	*size = _size;

	return _externalData;
}