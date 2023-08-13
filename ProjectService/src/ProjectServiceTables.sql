create table projects
(
    projectid uniqueidentifier not null Primary Key,
    projectname varchar(50) not null unique,
    projectdescription varchar(100),
    workflowid uniqueidentifier,
    projectstatus varchar(20),
    ownerid uniqueidentifier,
    created datetime2,
    modified datetime2,
    createdby nvarchar(20),
    modifiedby nvarchar(20)
)

create table componets
(
    componetid uniqueidentifier not null,
    projectid uniqueidentifier not null ,
    componentname varchar(50) not null ,
    componentdescription varchar(100),
    created datetime2,
    modified datetime2,
    createdby nvarchar(20),
    modifiedby nvarchar(20),
    CONSTRAINT PK_componet PRIMARY KEY (componetid,projectid),
    CONSTRAINT FK_Project_componet FOREIGN KEY (projectid)
    REFERENCES Project(projectid)
)

create table projectusers
(
    userid uniqueidentifier not null,
    projectid uniqueidentifier not null ,
    created datetime2,
    modified datetime2,
    createdby nvarchar(20),
    modifiedby nvarchar(20),
    userrole varchar(20),
    CONSTRAINT PK_componet PRIMARY KEY (userid,projectid),
    CONSTRAINT FK_Project_User FOREIGN KEY (projectid)
    REFERENCES Project(projectid)
)

CREATE INDEX idx_projectusers_projectid ON projectusers(projectid);
CREATE INDEX idx_projectusers_userid ON projectusers(userid);
CREATE INDEX idx_components_projectid ON components(projectid);
CREATE INDEX idx_components_componentname ON components(componentname);
CREATE INDEX idx_projects_projectname ON projects(projectname);
CREATE INDEX idx_projects_workflowid ON projects(workflowid);
CREATE INDEX idx_projects_projectstatus ON projects(projectstatus);
