/*
* Developed by Michael-Evangelos Diamantis Aug-2024
* for SAE Athens CMN6302 - Major.
* Source: https://github.com/MichaelEvangelosD/cmn6302_majorSAE
*/

#include "pch.h"

/*
A struct for incoming data handling.
Used solemly for data caching, packing and unpacking.
*/
struct DataContainer
{
	/*The saved data cache parent SMRI*/
	unsigned int _Smri;
	/*The cached data array size*/
	int _DataSize;
	/*The cached data converted in unsigned char for serialization*/
	std::vector<unsigned char> _DataValues;
	/*The referenced SMRI values this SMRI has.*/
	std::vector<int> _RefSmris;

	/*Packing msgpack::cppack method*/
	template<class T>
	void pack(T& pack) {
		pack(_Smri, _DataSize, _DataValues, _RefSmris);
	}
};

/*
A struct used solemly for packing and unpacking purposes.
Helps in making DataContainer serialization and deserialization easier.
*/
struct Data
{
	/*The map containing the smri-data pairs meant for serialization and deserialization.*/
	std::map<unsigned int, DataContainer> _ModelsCache;

	/*Packing msgpack::cppack method*/
	template<class T>
	void pack(T& pack) {
		pack(_ModelsCache);
	}
};

#pragma region GlobalVariables
/*The save format of the saved files.*/
const std::string SAVE_FORMAT = "{date}_{cnt}";
/*The save file extension*/
const std::string SAVE_EXTENSION = ".sav";
/*
The global SMRI value used in data storage and reference preservation.
Default value is -1
*/
int _GlobalSmriValue = -1;
/*A map storing SMRI-Data pairs. Used in serialization and deserialization.*/
std::map<unsigned int, DataContainer> _ModelsCache;
/*The externally-set absolute save path.*/
std::string _SavePath = "";
/*The externally-set file name to unpack from.*/
std::string _LoadFile = "";
#pragma endregion

#pragma region SavePath
/*
Sets the library save path variable.
The final directory gets created if it does not already exist.

@param const char* _savePath:
	A string containing the absolute save path to a directory.
@return	OperationSuccessful: Successful set of the path
@return	DirectoryNotFound: If the directory is not found
@return	OperationFailed: In case of any other error
*/
short setSavePath(const char* _savePath) {
	try {
		handleSaveDirectory(_savePath);

		_SavePath = _savePath;
		return OperationSuccessful;
	}
	catch (std::runtime_error) {
		return DirectoryNotFound;
	}
	catch (...) {
		return OperationFailed;
	}
}

/*
Returns the stored save directory absolute path.
@return The stored save path as a C string.
@return Nullptr in any other case
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
In case of any error, call decreaseSmri().
@return The current SMRI value incremented by 1.
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
Deletes the associated SMRI Data Container from the ModelCache.
@param unsigned int _smri
	The SMRI to delete
@return OperationSuccessful: Deletion was successful
@return OperationFailed: Any other error.
*/
short deleteSmriData(unsigned int _smri) {
	try {
		_ModelsCache.erase(_smri);
		return OperationSuccessful;
	}
	catch (...) {
		return OperationFailed;
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
That's the main data caching function of the library.
Creates and caches the DataContainer with the passed parameters and saves it in the ModelCache.
Passed _data are converted into unsigned char values.
@param unsigned int _smri: The smri to associate with the data.
@param int _dataSize: The size of the marshalled data.
@param unsigned char* _data: Pointer to the marshalled data array (byte array)
@param int _refsSize: The int ref smri array size
@param int* _refSmris: Pointer to the marshalled int array containing the ref SMRIs of this SMRI.
@return OperationSuccessful: If the caching and convertion were successful
@return OperationFailed: If any error occurs.
*/
short cacheData(unsigned int _smri, int _dataSize, unsigned char* _data, int _refsSize, int* _refSmris) {
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

		_ModelsCache[data._Smri] = data;

		return OperationSuccessful;
	}
	catch (...) {
		return OperationFailed;
	}
}

/*
Returns a pointer to the passed _smri associated data array.
@param unsigned int _smri: The SMRI to retrieve data from
@param int* _size: Is an output parameter and will be set with the array size of the returned data.
@return A pointer to the beginning of the SMRI associated data.
@return A nullptr in any other case.
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
Returns a pointer to the referenced SMRI values of the passed _parentSmri;
@param unsigned int _parentSmri: The SMRI to retrieve the referenced SMRIs array from.
@param  int* _size: Is an output parameter and will be set with the array size of the returned data.
@return int* A pointer to the beginning of the Reference SMRI int array.
@return A nullptr in any other case.
*/
int* getRefSmris(unsigned int _parentSmri, int* _size) {
	try {
		*_size = _ModelsCache.at(_parentSmri)._RefSmris.size();
		return _ModelsCache.at(_parentSmri)._RefSmris.data();
	}
	catch (...) {
		return nullptr;
	}
}

/*
The main serialization method of the library.
The whole _ModelCache gets serialized and stored in the set save path along with a dynamically created name controlled from the SAVE_FORMAT variable.
The serialization is handled from the msgpack hpp library.
@return OperationSuccessful: If the serialization was successful
@return OperationFailed: If any error occurs.
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

		return OperationSuccessful;
	}
	catch (...) {
		return OperationFailed;
	}
}
#pragma endregion

#pragma region LoadFromFile_Unpacking
/*
Sets the file name to read data to unpack from which must reside inside the set _SavePath.
The name gets validated for its existance inside the _SavePath every time it is set.
@param const char* _loadFileName: A string containing the file name to load from.
@throw std::runtime_error: Passed _LoadFile does not exist in the saves folder.
@return OperationSuccessful: If the name was successfully set
@return FileNotFound: If the file was not found inside the _SavePath directory.
@return OperationFailed: If any other error occurs.
*/
short setLoadFileName(const char* _loadFileName) {
	try {
		std::string comp = combinePath(_SavePath, _loadFileName);
		if (!fileExists(comp)) {
			throw std::runtime_error("Passed _LoadFile does not exist in the saves folder.");
		}

		_LoadFile = _loadFileName;
		return OperationSuccessful;
	}
	catch (std::runtime_error) {
		return FileNotFound;
	}
	catch (...) {
		return OperationFailed;
	}
}

/*
Returns the library cached _LoadFile value containing the file name to unpack data from.
@return A C string with the value of the _LoadFile
@return A nullptr in any other case.
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
The deserialization function of the library.
The serialized data are read directly from the save path and file name, get deserialized and
cached inside the _ModelsCache library map.
The GlobalSMRI is set to be equal to the size of the deserialized data map.
@return CouldNotOpenFile: In case the file could not be opened.
@return ReadNotSuccessful: In case the file could not be read.
@return OperationSuccessful: In case the whole process was successful.
@return OperationFailed: In any other error.
*/
short unpackData() {
	try {

		std::ifstream dataFile(combinePath(_SavePath, _LoadFile), std::ios::binary);
		if (!dataFile.is_open()) {
			//Could not open file
			return CouldNotOpenFile;
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
			return ReadNotSuccessful;
		}

		Data container = msgpack::nvp_unpack<Data>(bytes); //Deserialization
		for (const std::pair<unsigned int, DataContainer>& pair : container._ModelsCache) {
			_ModelsCache[pair.first] = pair.second;
		}

		//Size must be set here in case we unpack a cache so the rest SMRIs register correctly.
		int sz = _ModelsCache.size() - 1;
		_GlobalSmriValue = sz <= 0 ? resetSmri() : sz;

		return OperationSuccessful;
	}
	catch (...) {
		return OperationFailed;
	}
}
#pragma endregion

#pragma region DLL_Cleanup
/*
Resets the global SMRI back to its default value: -1
@return OperationSuccessful if the operation was successful.
@return OperationFailed: In any other error.
*/
short resetSmri() {
	try {
		_GlobalSmriValue = -1;
		return OperationSuccessful;
	}
	catch (...) {
		return OperationFailed;
	}
}

/*
Clears the ModelsCache, _SavePath and _LoadFile memory.
@return OperationSuccessful If the operation was successful.
@return OperationFailed: In any other error.
*/
short resetCache() {
	try {
		_ModelsCache.clear();
		_SavePath = "";
		_LoadFile = "";
		return OperationSuccessful;
	}
	catch (...) {
		return OperationFailed;
	}
}
#pragma endregion