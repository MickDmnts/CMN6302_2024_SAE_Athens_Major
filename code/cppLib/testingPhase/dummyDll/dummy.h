#pragma once

#ifdef DUMMY_EXPORTS
#define DUMMY_API __declspec(dllexport)
#else
#define DUMMY_API __declspec(dllimport)
#endif

extern "C" DUMMY_API void setNum(unsigned int num);

extern "C" DUMMY_API unsigned int getNum();

extern "C" DUMMY_API bool isItCool(bool state);

extern "C" DUMMY_API void cacheByteArray(unsigned char* data, int size);

extern "C" DUMMY_API unsigned char* getByteArray(int* size);