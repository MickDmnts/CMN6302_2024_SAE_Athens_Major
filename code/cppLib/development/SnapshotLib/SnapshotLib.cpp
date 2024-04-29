/*
* Developed by Michael-Evangelos Diamantis Aug-2024
* for SAE Athens CMN6302 - Major.
* Source: https://github.com/MichaelEvangelosD/cmn6302_majorSAE
*/

#include "pch.h"
#include "SnapshotLib.h"
#include "LibraryUtils.h"

/*
@TODO: Summary
*/
struct Data
{
	std::unordered_map<unsigned int, DataContainer> _ModelsCache;

	template<class T>
	void pack(T& pack) {
		pack(_ModelsCache);
	}
};

#pragma region GlobalVariables
/*
@TODO: Summary
*/
const std::string SAVE_FORMAT = "{date}_{cnt}";
/*
@TODO: Summary
*/
const std::string SAVE_EXTENSION = ".sav";
/*
The global SMRI value used in data storage and reference preservation.
Default value is -1
*/
int _GlobalSmriValue = -1;
/*
TODO: Summary - SMRI, (Size of data, Data)
*/
std::unordered_map<unsigned int, DataContainer> _ModelsCache;
/*
@TODO: Summary
*/
std::string _SavePath = "";
#pragma endregion

/*
@TODO: Summary
*/
short setSavePath(const char* _savePath) {
	try {
		_SavePath = _savePath;
		if (!directoryExists(_SavePath)) {
			throw std::runtime_error("Passed _SavePath does not exist.");
		}
		return 0;
	}
	catch (std::runtime_error) {
		return 76;
	}
	catch (...) {
		return 1;
	}
}

/*
@TODO: Summary
*/
const char* getSavePath() {
	try {
		return _SavePath.c_str();
	}
	catch (...) {
		return nullptr;
	}
}

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
@TODO: Summary
*/
short cacheData(DataContainer _model) {
	try {
		DataContainer data = DataContainer();
		data._Smri = _model._Smri;
		data._DataSize = _model._DataSize;
		data._Data = new unsigned char[data._DataSize];
		std::memcpy(data._Data, _model._Data, data._DataSize);

		_ModelsCache[_model._Smri] = data;

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
		*_size = _ModelsCache[_smri]._DataSize;
		return _ModelsCache[_smri]._Data;
	}
	catch (...) {
		return nullptr;
	}
}

/*
@TODO: Summary
*/
short packData() {
	try {
		Data container = Data();
		container._ModelsCache = _ModelsCache;
		std::vector<uint8_t> serData = msgpack::pack(container);

		int cnt = getFileCount(_SavePath);
		std::string dt = getCurrentDate();
		std::string finalSaveName = formatSaveString(SAVE_FORMAT, dt, cnt);

		std::string saveStr = combinePath(_SavePath, finalSaveName) + SAVE_EXTENSION;
		std::ofstream outfile(saveStr, std::ios::out);
		outfile.write(reinterpret_cast<const char*>(serData.data()), serData.size());
		outfile.close();
		return 0;
	}
	catch (...) {
		return 1;
	}
}

/*
@TODO: Summary
*/
short resetCache() {
	try {
		_ModelsCache.clear();
		return 0;
	}
	catch (...) {
		return 1;
	}
}