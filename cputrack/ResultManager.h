
#pragma once

#include "QueuedTracker.h"
#include <list>
#include "threads.h"



// Runs a seperate result fetching thread
class ResultManager
{
public:
	ResultManager(const char *outfile, const char *frameinfo, ResultManagerConfig *cfg, std::vector<std::string> colnames);
	~ResultManager();

	void SaveSection(int start, int end, const char *beadposfile, const char *infofile);
	void SetTracker(QueuedTracker *qtrk);
	QueuedTracker* GetTracker();

	int GetBeadPositions(int startFrame, int endFrame, int bead, LocalizationResult* r);
	int GetResults(LocalizationResult* results, int startFrame, int numResults);
	void Flush();

	RMFrameCounters GetFrameCounters();
	void StoreFrameInfo(int frame, double timestamp, float* columns); // return #frames
	int GetFrameCount();
	// Make sure that the space for that frame is allocated

	bool RemoveBeadResults(int bead);
	
	const ResultManagerConfig& Config() { return config; }

protected:
	bool CheckResultSpace(int fr);
	void Write();
	void WriteBinaryResults();
	void WriteTextResults();

	void StoreResult(LocalizationResult* r);
	static void ThreadLoop(void *param);
	bool Update();
	void WriteBinaryFileHeader();

	struct FrameResult
	{
		FrameResult(int nResult, int nFrameInfo) : frameInfo(nFrameInfo), results(nResult) { count=0; timestamp=0; hasFrameInfo=false;}
		std::vector<LocalizationResult> results;
		std::vector<float> frameInfo;
		int count;
		double timestamp;
		bool hasFrameInfo;
	};

	Threads::Mutex resultMutex, trackerMutex;

	std::vector<std::string> frameInfoNames;

	std::deque< FrameResult* > frameResults;
	RMFrameCounters cnt;
	ResultManagerConfig config;
	
	QueuedTracker* qtrk;

	std::string outputFile, frameInfoFile;
	Threads::Handle* thread;
	Atomic<bool> quit;

};
