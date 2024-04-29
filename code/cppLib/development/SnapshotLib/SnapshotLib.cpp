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
struct DataContainer
{
	unsigned int _Smri;
	int _DataSize;
	std::vector<unsigned char> _DataValues;
	std::vector<int> _RefSmris;

	template<class T>
	void pack(T& pack) {
		pack(_Smri, _DataSize, _DataValues);
	}
};

/*
@TODO: Summary
*/
struct Data
{
	std::map<unsigned int, DataContainer> _ModelsCache;

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
std::map<unsigned int, DataContainer> _ModelsCache;
/*
@TODO: Summary
*/
std::string _SavePath = "";
/*
@TODO: Summary
*/
std::string _LoadFile = "";
#pragma endregion

#pragma region SavePath
/*
@TODO: Summary
*/
short setSavePath(const char* _savePath) {
	try {
		if (!directoryExists(_savePath)) {
			throw std::runtime_error("Passed _SavePath does not exist.");
		}

		_SavePath = _savePath;
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
#pragma endregion

#pragma region SMRI_Handling
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
#pragma endregion

#pragma region DataCaching_Packing
/*
@TODO: Summary
*/
short cacheData(unsigned int _smri, int _dataSize, unsigned char* _data, int* _refSmris, int _refsSize) {
	try {
		DataContainer data = DataContainer();
		data._Smri = _smri;
		data._DataSize = _dataSize;
		//Data copying
		for (int i = 0; i < data._DataSize; ++i) {
			data._DataValues.push_back(_data[i]);
		}

		for (int i = 0; i < _refsSize; i++) {
			data._RefSmris.push_back(_refSmris[i]);
		}

		_ModelsCache[_smri] = data;

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
		*_size = _ModelsCache.at(_smri)._DataSize;
		return _ModelsCache.at(_smri)._DataValues.data();
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
		container._ModelsCache = std::map<unsigned int, DataContainer>(_ModelsCache);
		std::vector<uint8_t> serData = msgpack::nvp_pack(container); //Serialization

		int cnt = getFileCount(_SavePath);
		std::string dt = getCurrentDate();
		std::string finalSaveName = formatSaveString(SAVE_FORMAT, dt, cnt);

		std::string saveStr = combinePath(_SavePath, finalSaveName) + SAVE_EXTENSION;
		std::ofstream outfile(saveStr, std::ios::out | std::ios::binary);
		outfile.write(reinterpret_cast<const char*>(serData.data()), serData.size());
		outfile.close();
		return 0;
	}
	catch (...) {
		return 1;
	}
}
#pragma endregion

#pragma region LoadFromFile_Unpacking
/*
@TODO: Summary
*/
short setLoadFileName(const char* _loadFileName) {
	try {
		std::string comp = combinePath(_SavePath, _loadFileName);
		if (!fileExists(comp)) {
			throw std::runtime_error("Passed _LoadFile does not exist in the saves folder.");
		}

		_LoadFile = _loadFileName;
		return 0;
	}
	catch (std::runtime_error) {
		return 404;
	}
	catch (...) {
		return 1;
	}
}

/*
@TODO: Summary
*/
const char* getLoadFileName() {
	try {
		return _LoadFile.c_str();
	}
	catch (...) {
		return nullptr;
	}
}

/*
@TODO: Summary
*/
short unpackData() {
	try {

		std::ifstream dataFile(combinePath(_SavePath, _LoadFile), std::ios::binary);
		if (!dataFile.is_open()) {
			//Could not open file
			return 2;
		}

		//Sizing
		dataFile.seekg(0, std::ios::end);
		std::streampos fileSize = dataFile.tellg();
		dataFile.seekg(0, std::ios::beg);

		//File read
		std::vector<uint8_t> bytes(fileSize);
		dataFile.read(reinterpret_cast<char*>(bytes.data()), fileSize);

		if (!dataFile) {
			//Could not read file correctly
			return 3;
		}

		Data container = msgpack::nvp_unpack<Data>(bytes); //Deserialization
		for (const std::pair<unsigned int, DataContainer>& pair : container._ModelsCache) {
			_ModelsCache[pair.first] = pair.second;
		}

		return 0;
	}
	catch (...) {
		return 1;
	}
}
#pragma endregion

#pragma region DLL_Cleanup
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
short resetCache() {
	try {
		_ModelsCache.clear();
		_SavePath = "";
		_LoadFile = "";
		return 0;
	}
	catch (...) {
		return 1;
	}
}
#pragma endregion