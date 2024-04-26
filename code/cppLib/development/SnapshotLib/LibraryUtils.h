#pragma once

#include <string>
#include <filesystem>

namespace fs = std::filesystem;

int getFileCount(std::string _path);
std::string getCurrentDate();
std::string formatSaveString(const std::string& _format, const std::string& date, const int cnt);
std::string combinePath(const std::string _base, const  std::string _exte);
bool directoryExists(const std::string& path);