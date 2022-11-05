
{
	"General": [
			"Superscalar Factor": 						[1...16],
			"N of Rename Entries": 						[1...512],
			"N of Reorder Entries": 					[1...512],
			"Separate Decode and Dispatch":				[true/false]
		]
	
	"Execution": [
			"Execution Unit Architecture": 				[Standard, Simple, Complex, Custom],
			"Reservation Architecture": 				[Distributed, Centralized, Hybrid],
			"N of Reservation Stations per Buffer": 	[1...8],
			"N of Integer Execution Units": 			[1...8],
			"N of Floating Point Execution Units":    	[1...8],
			"N of Branch Execution Units":				[1...8],
			"N of Memory Execution Units":				[1...8]
		]
		
	"Memory/Branching": [
			"Memory Architecture": 						[L1,L2, and System],
			"Branch Misspeculation": 					[true/false],
			"% Speculative Accuracy": 					[0...100],
			"System Memory Latency [Cycles]":			[0...10000],
			"L1 Code Cache Latency [Cycles]":			[0...100],	
			"% Hitrate for L1 Code Cache":				[0...100],
			"L1 Data Cache Latency [Cycles]":			[0...100],	
			"% Hitrate for L1 Data Cache":				[0...100],
			"L2 Cache Latency [Cycles]":				[0...500],
			"% Hitrate for L2 Cache":					[0...100]
	]
}