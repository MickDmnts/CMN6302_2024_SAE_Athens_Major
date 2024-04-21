#include "pch.h"
#include "dummy.h"
//#include <vector>
#include <iostream>
#include <fstream>
//#include <filesystem>
#include <unordered_map> 

//int _number;
//unsigned char* _externalData = nullptr;
//int _size;
std::unordered_map<unsigned int, DataContainer> _Models;

short cacheModel(DataContainer* _model, int _dataSize) {
	try {
		DataContainer temp(_model->_Smri, _model->_Data, _dataSize);
		std::ofstream outfile("output.bin", std::ios::binary);
		outfile.write(reinterpret_cast<const char*>(temp._Data), sizeof(temp));
		outfile.close();

		_Models.insert(std::make_pair(temp._Smri, temp));

		return 0;
	}
	catch (...) {
		return 1;
	}
}

DataContainer getModelData(unsigned int _smri) {
	return _Models.at(_smri);
}

//int getNum()
//{
//	return _number;
//}
//
//void setNum(int num)
//{
//	_number = num + 1;
//}
//
//bool isItCool(bool state)
//{
//	return !state;
//}
//
//int cacheByteArray(unsigned char* externalData, int size)
//{
//	int result = -1;
//
//	try
//	{
//		_size = size;
//		_externalData = new unsigned char[_size];
//
//		memcpy(_externalData, externalData, _size);
//
//		result = 0;
//	}
//	catch (...)
//	{
//		result = 1;
//	}
//
//	return result;
//}
//
//unsigned char* getByteArray(int* size)
//{
//	*size = _size;
//
//	unsigned char* temp = new unsigned char[_size];
//	memcpy(temp, _externalData, _size);
//
//	return temp;
//}