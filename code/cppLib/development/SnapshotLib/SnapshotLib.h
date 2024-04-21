#pragma once

#ifdef SNAPSHOT_EXPORTS
#define SNAPSHOT_API __declspec(dllexport)
#else
#define SNAPSHOT_API __declspec(dllimport)
#endif

extern "C" SNAPSHOT_API int getNum();