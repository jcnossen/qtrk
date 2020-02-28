#include "std_incl.h"
#include "qtrk_c_api.h"
#include "QueuedCPUTracker.h"

QueuedTracker* CreateCPUTracker(QTrkSettings* cfg)
{
	return new QueuedCPUTracker(*cfg);
}

