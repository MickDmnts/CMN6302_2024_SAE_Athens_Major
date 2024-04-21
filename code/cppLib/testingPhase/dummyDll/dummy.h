#pragma once

#ifdef DUMMY_EXPORTS
#define DUMMY_API __declspec(dllexport)
#else
#define DUMMY_API __declspec(dllimport)
#endif

struct DataContainer
{
public:
	DataContainer(unsigned int _smri, unsigned char* _data, int _dataSize) {
		_Smri = _smri;
		_Data = new unsigned char[_dataSize];
		memcpy(_Data, _data, _dataSize);
	}

	unsigned int _Smri;
	unsigned char* _Data;
};

extern "C" DUMMY_API short cacheModel(DataContainer* _model, int _dataSize);

extern "C" DUMMY_API DataContainer getModelData(unsigned int _smri);

//extern "C" DUMMY_API void setNum(int num);
//
//extern "C" DUMMY_API int getNum();
//
//extern "C" DUMMY_API bool isItCool(bool state);
//
//extern "C" DUMMY_API int cacheByteArray(unsigned char* data, int size);
//
//extern "C" DUMMY_API unsigned char* getByteArray(int* size);