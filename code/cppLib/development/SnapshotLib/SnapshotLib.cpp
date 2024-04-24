#include "pch.h"
#include "SnapshotLib.h"
#include <unordered_map>
#include <iostream>
#include <fstream>

DataContainer::DataContainer(unsigned int _smri, unsigned char* _data, int _dataSize) {
	_Smri = _smri;
	_Data = new unsigned char[_dataSize];
	memcpy(_Data, _data, _dataSize);
}

/*
The global SMRI value used in data storage and reference preservation.
Default value is -1
*/
int _GlobalSmriValue = -1;

/*
TODO: Summary
*/
std::unordered_map<unsigned int, std::tuple<int, unsigned char*>> _ModelCache;

/*
Increases and returns the current global SMRI.
@return The current SMRI value incremented by 1
*/
unsigned int getSmri() {
	_GlobalSmriValue += 1;
	return _GlobalSmriValue;
}

/*
Decreases the Global SMRI by 1.
Caps to default value: -1
*/
void decreaseSmri() {
	_GlobalSmriValue -= 1;
	if (_GlobalSmriValue < -1) {
		_GlobalSmriValue = -1;
	}
}

/*
@return The current global SMRI without incrementing it.
*/
unsigned int getCurrentSmri() {
	return _GlobalSmriValue;
}

/*
Resets the global SMRI back to its default value: -1
@return 0 if the operation was successful, 1 otherwise.
*/
short resetSmri() {
	try {
		_GlobalSmriValue = -1;
		return 0;
	}
	catch (...) {
		return 1;
	}
}

/*

*/
short cacheData(DataContainer _model, int _dataSize) {
	try {
		DataContainer temp(_model._Smri, _model._Data, _dataSize);
		std::ofstream outfile("output.bin", std::ios::out | std::ios::binary);
		outfile.write(reinterpret_cast<char*>(temp._Data), _dataSize);
		outfile.close();

		_ModelCache.insert(std::make_pair(temp._Smri, std::tuple<int, unsigned char*>(_dataSize, temp._Data)));

		return 0;
	}
	catch (...) {
		return 1;
	}
}

/*
@TODO: Summary
*/
unsigned char* getData(unsigned int _smri, int* _size) {
	try {
		*_size = std::get<0>(_ModelCache.at(_smri));
		return std::get<1>(_ModelCache.at(_smri));
	}
	catch (...) {
		return nullptr;
	}
}

/*
@TODO: Summary
*/
short resetCache() {
	try {
		for (std::pair<const unsigned int, std::tuple<int, unsigned char*>>& pair : _ModelCache) {
			std::get<0>(pair.second) = 0;
			delete[] std::get<1>(pair.second);
		}

		_ModelCache.clear();
		return 0;
	}
	catch (...) {
		return 1;
	}
}