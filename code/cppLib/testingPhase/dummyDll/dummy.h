#pragma once

#include <string>

#ifdef DUMMY_EXPORTS
#define DUMMY_API __declspec(dllexport)
#else
#define DUMMY_API __declspec(dllimport)
#endif

extern "C" DUMMY_API void setNum(const unsigned int num);

extern "C" DUMMY_API unsigned int getNum();

extern "C" DUMMY_API bool isItCool(const bool state);