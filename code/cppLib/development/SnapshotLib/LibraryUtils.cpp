/*
* Developed by Michael-Evangelos Diamantis Aug-2024
* for SAE Athens CMN6302 - Major.
* Source: https://github.com/MichaelEvangelosD/cmn6302_majorSAE
*/
#include "pch.h"

/*
Returns the file count from inside the passed  path.
@param const std::string _path: A string containing the absolute path to a directory.
@throw fs::filesystem_error: Passed _path does not exist.
@return The file count from inside the passed _path.
*/
int getFileCount(const std::string _path) {
	int fileCount = 0;

	if (!fs::exists(_path)) {
		throw fs::filesystem_error("Passed _path does not exist.", std::filesystem::directory_entry(), std::make_error_code(std::errc::no_such_file_or_directory));
	}

	for (const auto& entry : fs::directory_iterator(_path)) {
		//Count only files
		if (entry.is_regular_file()) {
			fileCount++;
		}
	}

	return fileCount;
}

/*
@return The current date in a DD_MM_YYYY format as a string.
*/
std::string getCurrentDate() {
	time_t now;
	time(&now);

	struct tm current_time;
	localtime_s(&current_time, &now);

	std::stringstream ss;
	ss << std::setfill('0') << std::setw(2) << current_time.tm_mday << "_"
		<< std::setw(2) << current_time.tm_mon + 1 << "_"
		<< current_time.tm_year + 1900;

	return ss.str();
}

/*
@param const std::string& _format: The format to replace the values from.
@param const std::string& date: The date value to replace the {date}.
@param const int cnt: The count value to replace the {count}.
@return Replaces the date and count from the passed format and returns it.
e.g.: {date}_{count} - 02_03_2024_1
*/
std::string formatSaveString(const std::string& _format, const std::string& date, const int cnt) {
	std::string result = _format;
	std::size_t pos = result.find(_format);
	if (pos != std::string::npos) {
		result.replace(pos, _format.length(), date + "_" + std::to_string(cnt));
	}
	return result;
}

/*
Combines the passed strings with the corresponding system-relative path combination symbol.
No checking takes place.
@param const std::string _base: The base absolute path.
@param const std::string _exte: The extension relative path.
@return The combined path string.
*/
std::string combinePath(const std::string _base, const std::string _exte) {
	fs::path _comb = fs::path(_base) / fs::path(_exte);
	return _comb.string();
}

/*
Creates the passed directory if it does not exist.
@param const std::string& path: The directory absolute path.
*/
void handleSaveDirectory(const std::string& path) {
	fs::path directoryPath(path);

	if (!fs::exists(directoryPath)) {
		fs::create_directory(directoryPath);
	}
}

/*
@param const std::string& path: Absolute path to the file.
@return True if the file exists, false otherwise.
*/
bool fileExists(const std::string& path) {
	fs::path directoryPath(path);

	return fs::exists(directoryPath);
}