/*
* Developed by Michael-Evangelos Diamantis Aug-2024
* for SAE Athens CMN6302 - Major.
* Source: https://github.com/MichaelEvangelosD/cmn6302_majorSAE
*/
#pragma once

#include <string>
#include <filesystem>
#include <sstream>
#include <iostream>
#include <ctime>
#include <iomanip>
#include <sstream>

namespace fs = std::filesystem;

int getFileCount(std::string _path);
std::string getCurrentDate();
std::string formatSaveString(const std::string& _format, const std::string& date, const int cnt);
std::string combinePath(const std::string _base, const  std::string _exte);
bool directoryExists(const std::string& path);