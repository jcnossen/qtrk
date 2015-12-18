#include "std_incl.h"
#include "QueuedTracker.h"
#include "BeadFinder.h"

CDLL_EXPORT void DLL_CALLCONV TestDLLCallConv(int a)
{

}

CDLL_EXPORT void DLL_CALLCONV QTrkGetDefaultConfig(QTrkSettings* cfg)
{
	*cfg = QTrkSettings();
}

CDLL_EXPORT void DLL_CALLCONV QTrkGetComputedConfig(QTrkSettings* base, QTrkComputedConfig* cfg)
{
	*cfg=QTrkComputedConfig(*base);
}


CDLL_EXPORT void QTrkFreeROIPositions(ROIPosition *data)
{
	delete[] data;
}

CDLL_EXPORT ROIPosition* QTrkFindBeads(float* image, int w,int h, int smpCornerPosX, int smpCornerPosY, int roi, float imgRelDist, float acceptance)
{
	BeadFinder::Config cfg;
	cfg.img_distance = imgRelDist;
	cfg.roi = roi;
	cfg.similarity = acceptance;
	ImageData img = ImageData(image, w,h);
	ImageData sampleImg = img.subimage(smpCornerPosX, smpCornerPosY, roi,roi);
	auto results = BeadFinder::Find(&img, sampleImg.data, &cfg);
	sampleImg.free();

	ROIPosition *output=new ROIPosition[results.size()];
	for (int i=0;i<results.size();i++)
	{
		output[i].x = results[i].x;
		output[i].y = results[i].y;
	}

	return output;
}



CDLL_EXPORT QueuedTracker* DLL_CALLCONV QTrkCreateInstance(QTrkSettings *cfg)
{
	return CreateQueuedTracker(*cfg);
}

CDLL_EXPORT void DLL_CALLCONV QTrkFreeInstance(QueuedTracker* qtrk)
{
	delete qtrk;
}


// C API, mainly intended to allow binding to .NET
CDLL_EXPORT void DLL_CALLCONV QTrkSetLocalizationMode(QueuedTracker* qtrk, LocMode_t locType)
{
	qtrk->SetLocalizationMode(locType);
}

// Frame and timestamp are ignored by tracking code itself, but usable for the calling code
// Pitch: Distance in bytes between two successive rows of pixels (e.g. address of (0,0) -  address of (0,1) )
// ZlutIndex: Which ZLUT to use for ComputeZ/BuildZLUT
CDLL_EXPORT void DLL_CALLCONV QTrkScheduleLocalization(QueuedTracker* qtrk, void* data, int pitch, QTRK_PixelDataType pdt, const LocalizationJob *jobInfo)
{
	qtrk->ScheduleLocalization(data,pitch,pdt,jobInfo);
}

CDLL_EXPORT void DLL_CALLCONV QTrkClearResults(QueuedTracker* qtrk)
{
	qtrk->ClearResults();
}

CDLL_EXPORT void DLL_CALLCONV QTrkFlush(QueuedTracker* qtrk)
{
	qtrk->Flush();
}

// Schedule an entire frame at once, allowing for further optimizations
CDLL_EXPORT int DLL_CALLCONV QTrkScheduleFrame(QueuedTracker* qtrk, void *imgptr, int pitch, int width, int height, ROIPosition *positions, int numROI, QTRK_PixelDataType pdt, const LocalizationJob *jobInfo)
{
	return qtrk->ScheduleFrame(imgptr, pitch, width, height, positions, numROI, pdt, jobInfo);
}


// data can be zero to allocate ZLUT data. zcmp has to have 'zlut_radialsteps' elements
CDLL_EXPORT void DLL_CALLCONV QTrkSetRadialZLUT(QueuedTracker* qtrk, float* data, int count, int planes)
{
	qtrk->SetRadialZLUT(data, count, planes);
}

CDLL_EXPORT void DLL_CALLCONV QTrkGetRadialZLUT(QueuedTracker* qtrk, float* dst)
{
	qtrk->GetRadialZLUT(dst);
}

CDLL_EXPORT void DLL_CALLCONV QTrkGetRadialZLUTSize(QueuedTracker* qtrk, int* count, int* planes, int* radialsteps)
{
	qtrk->GetRadialZLUTSize(*count, *planes, *radialsteps);
}

CDLL_EXPORT void DLL_CALLCONV QTrkSetRadialWeights(QueuedTracker*qtrk, float* zcmp)
{
	qtrk->SetRadialWeights(zcmp);
}


CDLL_EXPORT void DLL_CALLCONV QTrkBeginLUT(QueuedTracker* qtrk, uint flags)
{
	qtrk->BeginLUT(flags);
}

CDLL_EXPORT void DLL_CALLCONV QTrkBuildLUTFromFrame(QueuedTracker* qtrk, ImageData* frame, QTRK_PixelDataType pdt, int plane, ROIPosition* roipos, int numroi)
{
	ImageData extracted = ImageData::alloc(qtrk->cfg.width, qtrk->cfg.height*numroi);
	for (int i=0;i<numroi;i++){
		frame->copyTo(extracted, roipos[i].x, roipos[i].y, 0, i*qtrk->cfg.height, qtrk->cfg.width,qtrk->cfg.height);
	}
	qtrk->BuildLUT(extracted.data, sizeof(float)*qtrk->cfg.width, QTrkFloat, plane);
	extracted.free();
}

CDLL_EXPORT void DLL_CALLCONV QTrkBuildLUT(QueuedTracker* qtrk, void* data, int pitch, QTRK_PixelDataType pdt, int plane, vector2f* known_pos)
{
	qtrk->BuildLUT(data, pitch, pdt, plane, known_pos);
}

CDLL_EXPORT void DLL_CALLCONV QTrkFinalizeLUT(QueuedTracker* qtrk)
{
	qtrk->FinalizeLUT();
}

	
CDLL_EXPORT int DLL_CALLCONV QTrkGetResultCount(QueuedTracker* qtrk)
{
	return qtrk->GetResultCount();
}

CDLL_EXPORT int DLL_CALLCONV QTrkFetchResults(QueuedTracker* qtrk, LocalizationResult* results, int maxResults)
{
	return qtrk->FetchResults(results, maxResults);
}


CDLL_EXPORT int DLL_CALLCONV QTrkGetQueueLength(QueuedTracker* qtrk, int *maxQueueLen)
{
	return qtrk->GetQueueLength(maxQueueLen);
}

CDLL_EXPORT bool DLL_CALLCONV QTrkIsIdle(QueuedTracker* qtrk)
{
	return qtrk->IsIdle();
}

CDLL_EXPORT void DLL_CALLCONV QTrkGetProfileReport(QueuedTracker* qtrk, char *dst, int maxStrLen)
{
	strncpy(dst, qtrk->GetProfileReport().c_str(), maxStrLen);
}

CDLL_EXPORT void DLL_CALLCONV QTrkGetWarnings(QueuedTracker* qtrk, char *dst, int maxStrLen)
{
	strncpy(dst, qtrk->GetWarnings().c_str(), maxStrLen);
}

