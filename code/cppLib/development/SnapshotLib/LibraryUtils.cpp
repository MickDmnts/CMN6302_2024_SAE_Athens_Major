/*
* Developed by Michael-Evangelos Diamantis Aug-2024
* for SAE Athens CMN6302 - Major.
* Source: https://github.com/MichaelEvangelosD/cmn6302_majorSAE
*/
#include "pch.h"
#include "LibraryUtils.h"

/*
@TODO: Summary
*/
int getFileCount(const std::string path) {
	int fileCount = 0;

	for (const auto& entry : fs::directory_iterator(path)) {
		//Count only files
		if (entry.is_regular_file()) {
			fileCount++;
		}
	}

	return fileCount;
}

/*
@TODO: Summary
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
@TODO: Summary
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
@TODO: Summary
*/
std::string combinePath(const std::string _base, const  std::string _exte) {
	fs::path _comb = fs::path(_base) / fs::path(_exte);
	return _comb.string();
}

/*
@TODO: Summary
*/
bool directoryExists(const std::string& path) {
	fs::path directoryPath(path);

	return fs::exists(directoryPath) && fs::is_directory(directoryPath);
}