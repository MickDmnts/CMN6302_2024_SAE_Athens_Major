#pragma once

#ifdef SNAPSHOT_EXPORTS
#define SNAPSHOT_API __declspec(dllexport)
#else
#define SNAPSHOT_API __declspec(dllimport)
#endif

struct DataContainer
{
public:
	DataContainer(unsigned int _smri, unsigned char* _data, int _dataSize);

	unsigned int _Smri;
	unsigned char* _Data;
};

extern "C" SNAPSHOT_API unsigned int getSmri();
extern "C" SNAPSHOT_API void decreaseSmri();
extern "C" SNAPSHOT_API unsigned int getCurrentSmri();
extern "C" SNAPSHOT_API short resetSmri();
extern "C" SNAPSHOT_API short cacheData(DataContainer _model, int _dataSize);
extern "C" SNAPSHOT_API unsigned char* getData(unsigned int _smri, int* _size);
extern "C" SNAPSHOT_API short resetCache();