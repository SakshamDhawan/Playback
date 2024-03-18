SubDatae=dir('/Users/Reneira/Desktop/Back_pain_Data/ALL_DATA_TOBEREDONE/BEHAVIOURAL_DATA/all/DATA_TO_BE_CHANGED/2/mat/mat/*.mat'); %emg data path

SubDatam=dir('/Users/Reneira/Desktop/Back_pain_Data/ALL_DATA_TOBEREDONE/BEHAVIOURAL_DATA/all/DATA_TO_BE_CHANGED/2/csv/csv_halves/csv_first_halves/*.csv'); %mmg data path
%%
%Loop through each emg and corresponding mmg file - extract raw data and
%pass through extract_all function

%The extract all function passes a bandpass filter over the data, rectifies
%it and also passes a lowpass filter over the data, which provides a smooth version
%of the signal to allow correlations to be calculated
%A hampel filter is applied to the MMG as an extra step to remove large
%spikes in the signal caused from large motion
%the extract all function returns the mmg and emg signals after each
%preprocessing step


%EMG_filt/MMG_filt = first step applied (bandpass filter)

%EMG_rect/MMG_rect = second step applied (rectification)

%MMG_h = third step only for MMG (hampel filter)

%EMG_smooth/MMG_smooth = last preprocessing step with low pass filter


%for i=1:10
EMG_data=readmatrix([SubDatae(i).folder '/' SubDatae(i).name],'Delimiter',',');
%IMU_Rawdata=load([SubDatam(i).folder '/' SubDatam(i).name]);


EMG_data(isnan(EMG_data)|isinf(EMG_data))=0;

[EMG_smooth,EMG_rect,EMG_filt]= extract_all_wless_emg_ORIG_BEHAV(EMG_data);

%filename= [SubDatae(i).folder '/' SubDatae(i).name];
%filename=[filename(1:end-4) '_extracted.mat'];

%save(filename,'MMG_smooth','exer','MMG_h','MMG_rect','MMG_filt','Torso_raw_acc','EMG_smooth','EMG_rect','EMG_filt', 'total_acc')
  
%clearvars -except SubDatae SubDatam i
%end


%%


SubDatae=dir('/Users/Reneira/Desktop/Back_pain_Data/ALL_DATA_TOBEREDONE/BEHAVIOURAL_DATA/all/DATA_TO_BE_CHANGED/3/extra/*.mat');
%%
for i=1:5
%% 
i=13;

load([SubDatae(i).folder '/' SubDatae(i).name]);
%%
%[Xa,Ya,D] = alignsignals(EMG_smooth(:,2),MMG_smooth(:,2));

D=1505000;
if D>0
    EMG_smooth = [zeros(D,3); EMG_smooth];
    EMG_rect = [zeros(D,3); EMG_rect];
    EMG_filt = [zeros(D,3); EMG_filt];

    if length(EMG_smooth)>length(MMG_smooth)
        EMG_smooth(length(MMG_smooth)+1:end,:)=[];
        EMG_rect(length(MMG_smooth)+1:end,:)=[];
        EMG_filt(length(MMG_smooth)+1:end,:)=[];

    else
        exer(length(EMG_smooth)+1:end)=[];
        MMG_smooth(length(EMG_smooth)+1:end,:)=[];
        MMG_h(length(EMG_smooth)+1:end,:)=[];
        MMG_filt(length(EMG_smooth)+1:end,:)=[];
        MMG_rect(length(EMG_smooth)+1:end,:)=[];
        Torso_raw_acc(length(EMG_smooth)+1:end,:)=[];
        total_acc(length(EMG_smooth)+1:end,:)=[];

    end

else
    MMG_smooth = [zeros(abs(D),3); MMG_smooth];
    MMG_h = [zeros(abs(D),3); MMG_h];
    MMG_filt = [zeros(abs(D),3); MMG_filt];
    MMG_rect = [zeros(abs(D),3); MMG_rect];
    
    Torso_raw_acc = [zeros(abs(D),3); Torso_raw_acc];
    total_acc = [zeros(abs(D),1); total_acc];
    exer = [zeros(abs(D),1); exer];

    if length(EMG_smooth)>length(MMG_smooth)
        EMG_smooth(length(MMG_smooth)+1:end,:)=[];
        EMG_rect(length(MMG_smooth)+1:end,:)=[];
        EMG_filt(length(MMG_smooth)+1:end,:)=[];

    else
        exer(length(EMG_smooth)+1:end)=[];
        MMG_smooth(length(EMG_smooth)+1:end,:)=[];
        MMG_h(length(EMG_smooth)+1:end,:)=[];
        MMG_rect(length(EMG_smooth)+1:end,:)=[];
        MMG_filt(length(EMG_smooth)+1:end,:)=[];
        Torso_raw_acc(length(EMG_smooth)+1:end,:)=[];
        total_acc(length(EMG_smooth)+1:end,:)=[];

    end    
end
%%
filename= [SubDatae(i).folder '/' SubDatae(i).name];
filename=[filename(1:end-14) '_aligned.mat'];
%
save(filename,'EMG_smooth','MMG_smooth','exer','MMG_h','EMG_rect','MMG_rect','EMG_filt','MMG_filt', 'total_acc','Torso_raw_acc')
%%
subplot 211
plot(EMG_smooth(:,3))
ylim([0 0.01])
yyaxis right
plot(exer(:,1))
subplot 212
plot(EMG_smooth(:,2))
%ylim([0 0.00005])
yyaxis right
plot(MMG_smooth(:,2))
%%
close all
clearvars -except SubDatae i
end
%%

SubDatae=dir('/Users/Reneira/Desktop/Back_pain_Data/New_data_oct/new_extracted/*.mat');
%%
i=10;

clearvars -except SubDatae i
%load('/Users/Reneira/Desktop/Back_pain_Data/New_data_oct/mat/L_W_13_10_extracted.mat')
load([SubDatae(i).folder '/' SubDatae(i).name]);
%%
subplot 211
plot(EMG_smooth(1:end,2))
ylim([0 0.005])
yyaxis right
plot(exer(:,1))
subplot 212
plot(EMG_smooth(:,1))
%ylim([0 0.05])
yyaxis right
plot(MMG_smooth(1253000:end,2))

%%

[Xa,Ya,D] = alignsignals(EMG_smooth(:,3),MMG_smooth(:,3));
%%
plot(Xa)
yyaxis right
plot(Ya)
%%

%%
%%
SubDatae=dir('/Users/Reneira/Desktop/Back_pain_Data/New_data_oct/Behav/*_aligned.mat');
i=1;
load([SubDatae(i).folder '/' SubDatae(i).name]);
%% E_A

Times{1}=[1,86374;86374,108390;108390,141400;267500,320000;320000,390000;390000,438100;438100,504965;
    504965,553300;553300,618000;721200,780417;820100,887700;887700,936000;936000,998500;998500,1042900;
    1042900,1113100;1160700,1206400;1206400,1269500;1269500,1313900;1346700,1413000;1413000,1463000;
    1463000,1535000;1535000,1747200;1747200,1926200];

% E_G missed out last bracing 4 and 8 sec
Times{2}=[119500,138800;138800,156100;164400,181000;198000,244100;244100,309400;338200,377000;377000,443600;
    443600,491000;491000,563100;563100,616900;616900,691400;691400,739000;739000,805600;805600,853600;
    853600,926800;926800,980200;980200,1050400;1050400,1100500;1100500,1172000;1172000,1242000;1242000,1323200];
% F_F

Times{3}=[135400,165900;165900,187400;200700,222500;264000,310200;310200,377500;377500,426600;426600,491800;507100,558800;
    558800,636500;636500,692100;692100,774000;774000,831000;831000,902100;902100,965600;994300,1059400;1169200,1214900;
    1214900,1311700;1311700,1361200;1361200,1428600;1428600,1482400;1482400,1549200;1549200,1628740;1628740,1768480];
% N_G

Times{4}=[44010,62650;62650,83800;83800,103800;113200,158300;178200,244600;244600,293000;
    310100,374500;453300,506400;568500,632500;756700,803200;803200,880100;880100,916600;
    916600,979600;979600,1023700;1023700,1085800;1173500,1218600;1218600,1283000;1283000,1330500;
    1330500,1397470;1397470,1444840;1444840,1508400;1530800,1615400;1615400,1686900];

% R_SS

Times{5}=[111000,135000;135000,163500;195200,215700;281000,331700;331700,398500;
    398500,445400;445400,509700;509700,555600;555600,619900;679400,729200;934000,999350;
    999350,1044700;1044700,1108600;1108600,1153400;1153400,1221200;1221200,1269000;1269000,1332900;
    1332900,1377000;1377000,1440700;1440700,1485550;1485550,1549900;1549900,1654220;1677020,1765190];
%% A_C

Times{6}=[41475,75158;100846,117359;142500,160478;304970,372400;699285,769900;821208,881392];
%% ADK

Times{7}=[63100,85800;85800,108500;122300,140990;377000,446000;495600,569800;627000,702400];

%% G_P

Times{7}=[321300,341900;341900,364800;385200,402900;564300,653800;726700,815300;833800,921900];
%% N_W

Times{7}=[133800,158200;189500,205800;226200,242600;353000,434600;463200,548900;548900,633200];

%% P_W

Times=[104500,125900;125900,148600;226800,246400;850600,917500;959700,1027900;1055000,1125000];

%% R_C

Times=[12800,28300;37100,52900;72700,88500;133700,199300;214800,280400;430700,495500];
%% R_M

Times=[163000,186000;186000,205600;221300,236400;520100,619900;687200,785700;1342500,1441000];

%%
SubDatae=dir('/Users/Reneira/Desktop/Back_pain_Data/New_data_oct/Behav/aligned/*_aligned.mat');
for i=1:5
    clearvars -except SubDatae Times RHOS i
    load([SubDatae(i).folder '/' SubDatae(i).name]);
    for a=1:length(Times{1,i})
        for sens=1:3
            e=EMG_smooth(Times{1,i}(a,1):Times{1,i}(a,2),sens);
            m=MMG_smooth(Times{1,i}(a,1):Times{1,i}(a,2),sens);
            
            [R,P]=corr(e,m,'Type','Spearman');
            rho(a,sens)=R;  
        end  
        
    end
    RHOS(:,:,i)=rho;
end

%%
for i=1:23
med(i)=median(RHOS(i,1,:));
end

%%
subplot 211
plot(exer)
yyaxis right
plot(EMG_smooth(:,1))
ylim([0 0.0005])

subplot 212
plot(exer)
yyaxis right
plot(EMG_smooth(:,2))
ylim([0 0.0005])
%%



%%
[istart,istop,dist]=findsignal(exer,template1,'MaxNumSegments',15);

[istart2,istop2,dist2]=findsignal(exer,template2,'MaxNumSegments',15);
%%

k=strfind(exer',template1')
%%
starts=sort([istart; istart2]);
stops=sort([istop; istop2]);

%%
for i=1:length(SubDatae)%[1:13 15:length(SubData)]% %[1:13 15:length(SubData)] %1:length(SubData) %1:6 % 7:15 %[8 10 12 13] %1:length(SubData)

load([SubDatae(i).folder '/' SubDatae(i).name]);


filename= [SubDatae(i).folder '/' SubDatae(i).name];
filename=[filename(1:end-4) '_extracted.mat'];

save(filename,'EMG_smooth','MMG_smooth','exer','MMG_h','EMG_rect')
  
clearvars -except SubDatae SubDatam i
end


%%

plot(EMG_smooth(:,1))
%ylim([0 0.00005])
yyaxis right
plot(MMG_smooth([1368500:end],1))
%ylim([0 0.05])

%%

MMG_smooth=MMG_smooth([210000:end],:);

MMG_h=MMG_h([210000:end],:);
exer=exer([210000:end],:);

%%
t=0:0.002:(length(EMG_smooth(:,1))-1)*0.002;
sin_x=EMG_smooth(:,1);
cos_x=MMG_smooth(:,1);
% Open a figure and crate the axes
figure
axes;
%
% STEP 1:
%
% Create and open the video object
vidObj = VideoWriter('SIN_X_COS_X.avi');
open(vidObj);
%
% Loop over the data to create the video
for i=1:500:length(t)
   % Plot the data
   %h(1)=plot(t(i),sin_x(i),'o','markerfacecolor','r','markersize',5);
   %hold on
   subplot 211
   plot(t(1:i),sin_x(1:i),'r')
   set(gca,'xlim',[0 5000],'ylim',[0 0.0005])

   subplot 212
   %set(gca,'xlim',[0 500],'ylim',[0 0.0005])
   plot(t(1:i),cos_x(1:i),'b')
   %h(2)=plot(t(i),cos_x(i),'o','markerfacecolor','b','markersize',5);
   set(gca,'xlim',[0 5000],'ylim',[0 0.05])
   %
   % STEP 2
   %
   % Get the current frame
   currFrame = getframe;
   %
   % STEP 3
   %
   % Write the current frame
   writeVideo(vidObj,currFrame);
   %
   delete(h)
end
%
% STEP 4
%
% Close (and save) the video object
close(vidObj);
