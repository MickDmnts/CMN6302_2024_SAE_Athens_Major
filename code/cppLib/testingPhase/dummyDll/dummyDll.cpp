// dummyDll.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "dummyDll.h"


// This is an example of an exported variable
DUMMYDLL_API int ndummyDll=0;

// This is an example of an exported function.
DUMMYDLL_API int fndummyDll(void)
{
    return 0;
}

// This is the constructor of a class that has been exported.
CdummyDll::CdummyDll()
{
    return;
}
