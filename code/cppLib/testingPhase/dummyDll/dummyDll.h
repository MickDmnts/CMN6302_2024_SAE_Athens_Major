// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the DUMMYDLL_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// DUMMYDLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef DUMMYDLL_EXPORTS
#define DUMMYDLL_API __declspec(dllexport)
#else
#define DUMMYDLL_API __declspec(dllimport)
#endif

// This class is exported from the dll
class DUMMYDLL_API CdummyDll {
public:
	CdummyDll(void);
	// TODO: add your methods here.
};

extern DUMMYDLL_API int ndummyDll;

DUMMYDLL_API int fndummyDll(void);
