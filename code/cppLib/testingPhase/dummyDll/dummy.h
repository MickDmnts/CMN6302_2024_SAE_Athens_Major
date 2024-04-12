#pragma once

#ifdef DUMMY_EXPORTS
#define DUMMY_API __declspec(dllexport)
#else
#define DUMMY_API __declspec(dllimport)
#endif

extern "C" DUMMY_API void setNum(int num);

extern "C" DUMMY_API int getNum();

extern "C" DUMMY_API bool isItCool(bool state);

extern "C" DUMMY_API int cacheByteArray(unsigned char* data, int size);

extern "C" DUMMY_API unsigned char* getByteArray(int* size);