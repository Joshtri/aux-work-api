namespace AuxWork.Api.Models;

public enum WorkType { Task, Bug, Feature }
public enum WorkStatus { Backlog, Ready, InProgress, InReview, Blocked, Done, Canceled }
public enum Priority { Critical, High, Medium, Low }
public enum MemberRole { Owner, Collaborator, ClientViewer }