import type {Application, ApplicationDetail} from './api';
// Fictional, fixed demonstration data. No accounts or live recruitment information.
export const sampleApplications:Application[]=[
 {id:1,company:'Infosys',position:'Associate Software Engineer',location:'Bengaluru',status:'Applied',appliedOn:'2026-10-13',followUpOn:'2026-11-02',notes:''},
 {id:2,company:'Accenture',position:'Application Development Associate',location:'Hyderabad',status:'Interview',appliedOn:'2026-10-06',followUpOn:'2026-11-01',notes:''},
 {id:3,company:'Cognizant',position:'Programmer Analyst Trainee',location:'Chennai',status:'Under Review',appliedOn:'2026-10-17',followUpOn:'2026-11-04',notes:''},
 {id:4,company:'Capgemini',position:'Junior Software Engineer',location:'Pune',status:'Rejected',appliedOn:'2026-09-30',followUpOn:null,notes:''},
 {id:5,company:'Deloitte',position:'Technology Analyst',location:'Bengaluru',status:'Saved',appliedOn:'2026-10-29',followUpOn:null,notes:''},
 {id:6,company:'TCS',position:'Assistant System Engineer',location:'Mumbai',status:'Under Review',appliedOn:'2026-10-22',followUpOn:'2026-11-03',notes:''}
];
export const sampleDetails:ApplicationDetail[]=sampleApplications.map(a=>({
 ...a,
 history:a.id===6?[{id:1,fromStatus:'',toStatus:'Applied',changedAt:'2026-10-22T08:00:00Z'},{id:2,fromStatus:'Applied',toStatus:'Under Review',changedAt:'2026-10-31T08:20:00Z'}]:[{id:a.id+10,fromStatus:'',toStatus:a.status,changedAt:'2026-10-20T09:00:00Z'}],
 interviews:a.id===2?[{id:1,round:'Technical interview',date:'2026-11-05',mode:'Online',notes:'C# and SQL fundamentals'}]:[]
}));
