function [EMG_smooth,EMG_rect,EMG_filt]= extract_all_wless_emg_ORIG_BEHAV(EMG_raw_data)

EMG_raw_data(isinf(EMG_raw_data)|isnan(EMG_raw_data)) = 0;

%Ti_EMG = 0:0.002:(length(EMG_ds(:,1))-1)*0.002;
%Ti_MMG = 0:0.002:(length(MMG_raw_data(:,1))-1)*0.002;


Fcp_low=10;  %lower cutoff frequency
Fcp_high=450;  %higher cutoff frequency
[z,p,k]=butter(4,[Fcp_low Fcp_high]/(2000/2),'bandpass');
[sos,g]=zp2sos(z,p,k);
EMG_filt=filtfilt(sos,g,EMG_raw_data);

EMG_filt=downsample(EMG_filt,4);

EMG_rect = abs(EMG_filt-mean(EMG_filt));
%




d1 = designfilt('lowpassiir','Halfpowerfrequency',0.0006,...
    'FilterOrder',1,'DesignMethod','Butter');


for i=1:size(EMG_rect,2)        
        EMG_smooth(:,i) = filtfilt(d1,EMG_rect(:,i));
end

end