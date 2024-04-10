#include "pch.h"
#include "dummy.h"
#include <iostream>

static unsigned int _number;

unsigned int getNum()
{
	return _number;
}

void setNum(const unsigned int num)
{
	_number = num+1;
}

bool isItCool(const bool state)
{
	return !state;
}