SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WorkQueueNotificationEvent](
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [WorkQueueNotificationEvent] ([Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'Vytvoření zprávy', NULL, NULL, N'fe40902f-8a44-477e-96f9-d157eee16a0f', CAST(0x00009E0E0176C49E AS DateTime), NULL)
INSERT [WorkQueueNotificationEvent] ([Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'1x denně', NULL, NULL, N'96340243-e360-4409-9ee6-26192748ead0', CAST(0x00009E0E0176C49E AS DateTime), NULL)
INSERT [WorkQueueNotificationEvent] ([Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'Zrušení zprávy', NULL, NULL, N'dc3b0836-b94f-4502-bb20-2c3aa3206f63', CAST(0x00009E0E0176C49E AS DateTime), NULL)
INSERT [WorkQueueNotificationEvent] ([Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'Eskalace', NULL, NULL, N'e1b66e46-7961-4d91-8c71-ab9e57e656a5', CAST(0x00009E0E0176C49F AS DateTime), NULL)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WorkQueueNotificationContactType](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[Name] [nvarchar](200) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [WorkQueueNotificationContactType] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'3535c6f5-c48d-4ae9-ba21-43852d4f66f8', CAST(0x00009E0E0176C4A1 AS DateTime), NULL, N'Vložená hodnota')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamTooltipHelpPosition](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[Name] [nvarchar](200) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [OrigamTooltipHelpPosition] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'41fa1c4f-9a12-4402-aa21-a921b0ceb52e', CAST(0x00009EB101249FC0 AS DateTime), NULL, N'Vlevo')
INSERT [OrigamTooltipHelpPosition] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'bdf7ca80-b110-4e16-932b-684f34c4fed3', CAST(0x00009EB101249FC0 AS DateTime), NULL, N'Nahoře')
INSERT [OrigamTooltipHelpPosition] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'c6f27d77-ab60-421c-b110-a2429a3e1c8b', CAST(0x00009EB101249FC0 AS DateTime), NULL, N'Vpravo')
INSERT [OrigamTooltipHelpPosition] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'da40379a-ae75-4982-888c-17205b64bf4d', CAST(0x00009EB101249FC0 AS DateTime), NULL, N'Dole')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamTooltipHelpGroup](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[Name] [nvarchar](200) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamTooltipHelpDestroyEvent](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[Name] [nvarchar](200) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [OrigamTooltipHelpDestroyEvent] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'59d08fb8-5f49-4a44-b9d9-0ef79aca413a', CAST(0x00009EB101249FC0 AS DateTime), NULL, N'Kliknutí')
INSERT [OrigamTooltipHelpDestroyEvent] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'77ddfc8d-57a9-4793-ba20-cc6e85d3575d', CAST(0x00009EB101249FC0 AS DateTime), NULL, N'Ručně')
INSERT [OrigamTooltipHelpDestroyEvent] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'f49d9d54-2f94-49b4-837a-735bf17c8b6e', CAST(0x00009EB10124A8EF AS DateTime), NULL, N'Výběr položky')
INSERT [OrigamTooltipHelpDestroyEvent] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'1341a433-94cd-4ae0-8e3f-f4949b80a2da', CAST(0x00009EB10124A8EF AS DateTime), NULL, N'Otevření formuláře')
INSERT [OrigamTooltipHelpDestroyEvent] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'3e6c58c8-df5d-4347-ab9e-4d425423ed26', CAST(0x00009EB10124AB3B AS DateTime), NULL, N'Vstup do pole')
INSERT [OrigamTooltipHelpDestroyEvent] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'3b701836-b4c0-4a0a-ac69-cc40e6057655', CAST(0x00009EB10124AB3B AS DateTime), NULL, N'Odchod z pole')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamTooltipHelpContext](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[IdString] [nvarchar](500) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [OrigamTooltipHelpContext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [IdString]) VALUES (NULL, NULL, N'9c453968-1dca-4b70-9474-1de935b2a343', CAST(0x00009EBC00CCAC25 AS DateTime), NULL, N'Panel', N'Origam.panels.GridPanel')
INSERT [OrigamTooltipHelpContext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [IdString]) VALUES (NULL, NULL, N'50446184-1abd-4bcf-89dd-1ce303213bf6', CAST(0x00009EBC00CCAC28 AS DateTime), NULL, N'Portál', N'Origam.portal.PortalUIInfrastructure')
INSERT [OrigamTooltipHelpContext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [IdString]) VALUES (NULL, NULL, N'9c03e73f-aee4-4b57-8be4-fda555f83394', CAST(0x00009EBC00CCAC28 AS DateTime), NULL, N'Záložky', N'Origam.panels.TabPage')
INSERT [OrigamTooltipHelpContext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [IdString]) VALUES (NULL, NULL, N'fc797cd0-2aa9-49ca-a3a4-891ffff237f0', CAST(0x00009EBC00CCAC28 AS DateTime), NULL, N'Nástroje', N'Origam.portal.tools.ToolsContainer')
INSERT [OrigamTooltipHelpContext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [IdString]) VALUES (NULL, NULL, N'13d7fa74-42a8-49b5-9ddf-caa887a39173', CAST(0x00009EBC00CCAC28 AS DateTime), NULL, N'Audit', N'Origam.portal.tools.AuditPanel')
INSERT [OrigamTooltipHelpContext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [IdString]) VALUES (NULL, NULL, N'b750974b-9999-40a8-bd7d-c8f976ec1e41', CAST(0x00009EBC00CCAC28 AS DateTime), NULL, N'Přílohy', N'Origam.portal.tools.AttachmentsPanel')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamTraceWorkflow](
	[WorkflowId] [uniqueidentifier] NOT NULL,
	[WorkflowName] [nvarchar](255) NOT NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamSyncSystem](
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamSyncRunLog](
	[ErrorText] [ntext] NULL,
	[DestinationId_guid] [uniqueidentifier] NULL,
	[SourceId_int] [int] NULL,
	[ReferenceCode] [nvarchar](50) NOT NULL,
	[DestinationId_int] [int] NULL,
	[refOrigamSyncRunId] [uniqueidentifier] NOT NULL,
	[IsSuccess] [bit] NOT NULL,
	[SourceId_guid] [uniqueidentifier] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_OrigamSyncRun] ON [OrigamSyncRunLog] 
(
	[refOrigamSyncRunId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_SourceGuid] ON [OrigamSyncRunLog] 
(
	[SourceId_guid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamSyncConnectionParameter](
	[Key] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](200) NULL,
	[refOrigamSyncConnectionId] [uniqueidentifier] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_OrigamSyncConnection] ON [OrigamSyncConnectionParameter] 
(
	[refOrigamSyncConnectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamStyleColor](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Color] [int] NOT NULL,
	[Description] [ntext] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_Unique] ON [OrigamStyleColor] 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'6a2cdf6c-b357-4a7c-8e6b-e18adcc597fd', NULL, NULL, N'portal-background-color-2011', 5000268, N'Portal: Background')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'f52cca95-9d05-463c-9d69-37e4e1cd9a70', NULL, NULL, N'portal-font-color-2011', 16777215, N'Portal: Font')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'e2bfb0a6-3e3b-4bdc-9920-42f93da98b27', NULL, NULL, N'button-mouse-over-background-color-2011', 9017541, N'Button: Mouse over background')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'864e018c-3003-46ee-8e34-c8e3b171a440', NULL, NULL, N'grid-selection-background-color-2011', 13421772, N'Grid: Selection background')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'5ea890ef-248e-4c22-8438-6a0b0a3d1029', NULL, NULL, N'grid-alternating-background-color-2011', 16250871, N'Grid: Alternating background')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'560577ff-f5aa-4ee8-892d-d821fd5fd930', NULL, NULL, N'grid-header-background-color-2011', 15066597, N'Grid: Header background')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'044b7dd9-9f82-48a6-a9bf-9064cd28b0f1', NULL, NULL, N'list-roll-over-background-color-2011', 12108252, N'List: Roll-over bakground')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'f487f794-6cb5-4789-a878-ee8dedb359d2', NULL, NULL, N'form-font-color-2011', 0, N'Form: Font')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'8ec9a6d8-14ab-4cd2-8150-ca63fb6c8efb', NULL, NULL, N'error-color-2011', 16711680, N'Error indicator')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'7a3e382d-93dc-4054-99cf-f33dcc9abf10', NULL, NULL, N'search-field-background-color-2011', 6710886, N'Portal: Search field background')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'46d8cf5e-6438-45a6-8513-bcf5f4ea6387', NULL, NULL, N'link-color-2011', 255, N'Link')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'9cb95ae9-06ab-47aa-806e-f73d5e80b580', NULL, NULL, N'button-mouse-down-background-color-2011', 4414371, N'Button: Mouse down background')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'028a6b2d-ea5d-4331-98b5-e2c903c55229', NULL, NULL, N'disabled-color-2011', 8421504, N'Input field: Disabled')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'9df50799-ac6d-4beb-8560-de874c5cb7b0', NULL, NULL, N'notification-holder-background-color-2011', 16775885, N'Form: Notification background')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'ce6178dd-ff33-464d-8b25-34d21b30f4b4', NULL, NULL, N'date-chooser-background-color-2011', 16775920, N'Input field: Date picker background')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'2188ffac-d863-4a5d-a565-e7ee07116930', NULL, NULL, N'disabled-border-color-2011', 12434877, N'Input field: Disabled border')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'508d9491-30b9-4a0d-9f9a-4b48e1bc9454', NULL, NULL, N'standard-border-color-2011', 16316664, N'Input field: Border')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'86403898-c3d5-4b7f-99f6-904b58d705e0', NULL, NULL, N'standard-border-space-color-2011', 14935011, N'Input field: Inactive border')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'2cd7774f-0db8-4f8e-8db9-58f30b26d309', NULL, NULL, N'mouse-over-border-color-2011', 9410501, N'Mouse-over border')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'a7822713-8ef1-4ac4-a44a-814b15727a77', NULL, NULL, N'workflow-next-button-color-2011', 13228684, N'Form: Screen-flow ¨Next button')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'bd5494ec-e958-42df-b9e1-00cc73632386', NULL, NULL, N'workflow-cancel-button-color-2011', 13667178, N'Form: Screen-flow Cancel button')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'1fe44aec-53e4-413a-aeb4-6bb158d3fae3', NULL, NULL, N'highlighted-search-color-2011', 16703866, N'Search result highlight')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'3654add7-2c42-44ad-9d37-c9bb8e1fb45c', NULL, NULL, N'drop-down-tree-roll-over-color-2011', 16448210, N'Input field: Drop-down tree roll-over')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'fcde84e7-d910-415e-b033-966ba64860ea', NULL, NULL, N'pipeline-default-background-color-2011', 15657130, N'Form: Pipeline default background')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'01674d1c-f768-4f97-b8a2-2e9e76ba4d69', NULL, NULL, N'audit-old-value-color-2011', 13947859, N'Audit: Old value')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'3c32fe0e-2872-466d-9114-c0e42394086b', NULL, NULL, N'audit-new-value-color-2011', 8906852, N'Audit: New value')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'3b380a87-147b-42f6-b8cc-5a69aad9f8b1', NULL, NULL, N'bubble-background-color-2011', 33040, N'Hint bubble background')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'6707bfad-727b-443a-bb16-fdccad258d14', NULL, NULL, N'palette-color-01-2011', 13667179, N'Palette 1')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'f7880aa5-ae12-44b6-a376-c9d54e633a54', NULL, NULL, N'palette-color-02-2011', 14395253, N'Palette 2')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'5a184d3f-3ef5-4248-b057-4e881087edc2', NULL, NULL, N'palette-color-03-2011', 15123327, N'Palette 3')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'85bce656-5906-4c67-a4e8-a4272e82af10', NULL, NULL, N'palette-color-04-2011', 15850888, N'Palette 4')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'ff489190-18c3-43c2-91f9-4332b2a9bb65', NULL, NULL, N'palette-color-05-2011', 16643985, N'Palette 5')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'395405a8-b114-44ed-ace6-db9133b4067e', NULL, NULL, N'palette-color-06-2011', 14936206, N'Palette 6')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'dd802e3d-a0e0-4f3e-9d4d-44ceec3b8fb0', NULL, NULL, N'palette-color-07-2011', 13228684, N'Palette 7')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'1fc8c39c-3c04-4298-aad5-41085bef161b', NULL, NULL, N'palette-color-08-2011', 11259273, N'Palette 8')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'fb06d8c7-acea-4fc1-b6d5-7060182c9473', NULL, NULL, N'palette-color-09-2011', 9616519, N'Palette 9')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'e01c11eb-e4c3-403f-9bc9-becfc3fa4a7c', NULL, NULL, N'palette-color-10-2011', 9683130, N'Palette 10')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'fbd7e703-2269-46db-8bec-ba46db1ac6bd', NULL, NULL, N'palette-color-11-2011', 9749482, N'Palette 11')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'1709eb28-7df9-4724-a48f-3a2b7b8798bf', NULL, NULL, N'palette-color-12-2011', 8952780, N'Palette 12')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'23ebdc4e-2325-4171-ba37-7410b9be3182', NULL, NULL, N'palette-color-13-2011', 8553915, N'Palette 13')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'a904329f-e20b-474c-b17e-4e86dda53d9e', NULL, NULL, N'palette-color-14-2011', 8155306, N'Palette 14')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'98ea3e75-6dc9-4587-9c3b-7de84f5bb0ba', NULL, NULL, N'palette-color-15-2011', 9599148, N'Palette 15')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'e512e9f8-9ed0-4f73-8c38-22fb90ed609c', NULL, NULL, N'palette-color-16-2011', 11108527, N'Palette 16')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'3368b444-4852-4519-a7a6-c0f07ad81568', NULL, NULL, N'palette-color-17-2011', 13799092, N'Palette 17')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'5e5c8e94-03e2-4144-a54b-4996cea69eae', NULL, NULL, N'palette-color-18-2011', 13733265, N'Palette 18')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'5205d18b-c5d8-4aee-8082-f7c02671e3e1', NULL, NULL, N'palette-color-19-2011', 12431510, N'Palette 19')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'f8aa9c0a-2861-488e-b366-08116858cd7c', NULL, NULL, N'palette-color-20-2011', 10721152, N'Palette 20')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'489fc524-3f44-4123-9f97-59368c4eb716', NULL, NULL, N'action-button-group-label-color-2011', 13421772, N'Portal: Action button group labels')
INSERT [OrigamStyleColor] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name], [Color], [Description]) VALUES (NULL, NULL, N'90de7a9a-a13a-45b2-8c53-2e9fe515f295', NULL, NULL, N'form-background-color-2011', 15066597, N'Form: Background')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamStateMachineEventType](
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamRoundingType](
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamPanelFilter](
	[PanelId] [uniqueidentifier] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[IsGlobal] [bit] NOT NULL,
	[ProfileId] [uniqueidentifier] NULL,
	[Name] [nvarchar](300) NOT NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_Main] ON [OrigamPanelFilter] 
(
	[PanelId] ASC,
	[ProfileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamPanelColumnConfig](
	[ProfileId] [uniqueidentifier] NOT NULL,
	[PanelId] [uniqueidentifier] NOT NULL,
	[ColumnName] [nvarchar](300) NOT NULL,
	[ColumnWidth] [int] NOT NULL,
	[Position] [int] NULL,
	[IsHidden] [bit] NOT NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_Main] ON [OrigamPanelColumnConfig] 
(
	[PanelId] ASC,
	[ProfileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamNotificationTemplate](
	[Template] [ntext] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamNotificationChannelType](
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [OrigamNotificationChannelType] ([Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'E-mail', NULL, NULL, N'3a0bbbb5-a7f0-4667-93d0-071f935702be', CAST(0x00009E0E0176C49D AS DateTime), NULL)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamModelVersion](
	[Version] [nvarchar](20) NULL,
	[refSchemaExtensionId] [uniqueidentifier] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[refSchemaExtensionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [OrigamModelVersion] ([Version], [refSchemaExtensionId], [DateUpdated]) VALUES (N'2.5', N'951f2cda-2867-4b99-8824-071fa8749ead', CAST(0x0000A2C700F231A1 AS DateTime))
INSERT [OrigamModelVersion] ([Version], [refSchemaExtensionId], [DateUpdated]) VALUES (N'4.11', N'147fa70d-6519-4393-b5d0-87931f9fd609', CAST(0x0000A35C015E866C AS DateTime))
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamMapLayerType](
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [OrigamMapLayerType] ([Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'Mapnik', NULL, NULL, N'1ee61797-3f5d-4fb1-9c50-e033dee563ab', CAST(0x0000A24B012BCE96 AS DateTime), NULL)
INSERT [OrigamMapLayerType] ([Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'WMS', NULL, NULL, N'da06ffa0-bcee-4923-aafe-b74945f2feaa', CAST(0x0000A24B012BCE9B AS DateTime), NULL)
INSERT [OrigamMapLayerType] ([Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'OSM', NULL, NULL, N'030a7bcf-c3f0-4009-844c-647640d99310', CAST(0x0000A35C015E8669 AS DateTime), NULL)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamRole](
	[Alias] [nvarchar](255) NULL,
	[Name] [nvarchar](255) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_Unique] ON [OrigamRole] 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
INSERT [OrigamRole] ([Alias], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (NULL, N'User', NULL, NULL, N'57642d8a-c110-47a7-93d5-21fda8886cf1', CAST(0x00009DAB017BC51C AS DateTime), NULL)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamRegularExpression](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Expression] [ntext] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamParameters](
	[FloatValue] [decimal](18, 10) NULL,
	[StringValue] [nvarchar](2000) NULL,
	[DateValue] [datetime] NULL,
	[BooleanValue] [bit] NOT NULL,
	[IntValue] [int] NULL,
	[GuidValue] [uniqueidentifier] NULL,
	[CurrencyValue] [money] NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamFeatureList](
	[Description] [ntext] NULL,
	[DisplayName] [nvarchar](255) NOT NULL,
	[ReferenceCode] [nvarchar](50) NOT NULL,
	[IsFeatureOn] [bit] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamCharacterTranslation](
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamMap](
	[ReferenceCode] [nvarchar](40) NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[InitialZoom] [int] NULL,
	[MapCenter] [geography] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_Unique] ON [OrigamMap] 
(
	[ReferenceCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE TYPE [OrigamListValue] AS TABLE(
	[ListValue] [nvarchar](max) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamCalendar](
	[IsThursdayWorkingDay] [bit] NOT NULL,
	[IsWednesdayWorkingDay] [bit] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[IsFridayWorkingDay] [bit] NOT NULL,
	[IsSundayWorkingDay] [bit] NOT NULL,
	[IsSaturdayWorkingDay] [bit] NOT NULL,
	[IsMondayWorkingDay] [bit] NOT NULL,
	[IsTuesdayWorkingDay] [bit] NOT NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamApplicationRole](
	[IsSystemRole] [bit] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[Description] [nvarchar](200) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_Name] ON [OrigamApplicationRole] 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
INSERT [OrigamApplicationRole] ([IsSystemRole], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Description]) VALUES (1, N'SYS_ExcelExport_Unlimited', NULL, NULL, N'566330eb-1b70-41f2-9def-efc873e987ee', CAST(0x0000A17700DD6CC5 AS DateTime), NULL, N'Funkce: Umožní uživateli exportovat neomezené množství záznamů')
INSERT [OrigamApplicationRole] ([IsSystemRole], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Description]) VALUES (1, N'WEB_Admin_AddUser', NULL, NULL, N'720342ed-176d-4734-9e1a-c3bf6521d212', CAST(0x0000A17700DD6CF9 AS DateTime), NULL, N'Web: Umožní přidávat nové uživatele pomocí webové služby')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamFavoritesTemplate](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[ConfigXml] [ntext] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamExceptionSeverity](
	[Name] [nvarchar](50) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamDataAuditLog](
	[OldValue] [nvarchar](max) NULL,
	[refColumnId] [uniqueidentifier] NOT NULL,
	[NewValueId] [nvarchar](50) NULL,
	[NewValue] [nvarchar](max) NULL,
	[OldValueId] [nvarchar](50) NULL,
	[ActionType] [int] NOT NULL,
	[refParentRecordEntityId] [uniqueidentifier] NULL,
	[refParentRecordId] [uniqueidentifier] NOT NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refParentRecordId] ON [OrigamDataAuditLog] 
(
	[refParentRecordId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamDashboardView](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[MenuId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Roles] [nvarchar](400) NOT NULL,
	[ConfigXml] [ntext] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_MenuId] ON [OrigamDashboardView] 
(
	[MenuId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Counter](
	[ReferenceCode] [nvarchar](40) NOT NULL,
	[Description] [nvarchar](300) NULL,
	[ManageValidityByDate] [bit] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_ReferenceCode] ON [Counter] 
(
	[ReferenceCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Attachment](
	[Note] [nvarchar](3900) NULL,
	[FileName] [nvarchar](3900) NULL,
	[refParentRecordEntityId] [uniqueidentifier] NULL,
	[refParentRecordId] [uniqueidentifier] NOT NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Data] [varbinary](max) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [ix_refParentRecordId] ON [Attachment] 
(
	[refParentRecordId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamWorkflowMenuItemConfig](
	[IsRepeatable] [bit] NOT NULL,
	[ProfileId] [uniqueidentifier] NOT NULL,
	[WorkflowMenuItemId] [uniqueidentifier] NOT NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_Main] ON [OrigamWorkflowMenuItemConfig] 
(
	[ProfileId] ASC,
	[WorkflowMenuItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamValidationRule](
	[Name] [nvarchar](200) NOT NULL,
	[Rule] [ntext] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_Unique] ON [OrigamValidationRule] 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DimensionEntity](
	[Name] [nvarchar](200) NOT NULL,
	[IsTarget] [bit] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Mail](
	[DateSent] [datetime] NOT NULL,
	[Sender] [nvarchar](500) NOT NULL,
	[Recipient] [ntext] NOT NULL,
	[Subject] [nvarchar](500) NULL,
	[DateReceived] [datetime] NULL,
	[MessageBody] [ntext] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Language](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[Name] [nvarchar](200) NULL,
	[TagIETF] [nvarchar](20) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DimensionType](
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RecurrenceDefinition](
	[WeeklySunday] [bit] NULL,
	[WeeklyWednesday] [bit] NULL,
	[Begin] [datetime] NULL,
	[End] [datetime] NULL,
	[RepeatInstances] [int] NULL,
	[WeeklyTuesday] [bit] NULL,
	[WeeklyThursday] [bit] NULL,
	[WeeklySaturday] [bit] NULL,
	[WeeklyFriday] [bit] NULL,
	[Frequency] [int] NULL,
	[RepeatForever] [bit] NULL,
	[WeeklyMonday] [bit] NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceAccessLevel](
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [ResourceAccessLevel] ([Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'Veřejné', NULL, NULL, N'69e9dc18-c37a-4a9a-90e2-6a07c628cc83', CAST(0x00009DAB0122EA9E AS DateTime), NULL)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WorkQueueExternalSourceType](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[Name] [nvarchar](200) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [WorkQueueExternalSourceType] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'57bed127-45cc-46f1-b29b-53c635f665b3', CAST(0x00009D1A0127AA38 AS DateTime), NULL, N'IMAP')
INSERT [WorkQueueExternalSourceType] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'07329b7b-90e8-4594-b738-c04856fc998e', CAST(0x00009DAB0122EA7C AS DateTime), NULL, N'File')
INSERT [WorkQueueExternalSourceType] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'4c15f1a1-4bd8-4fa6-9a37-df5aa19f02a5', CAST(0x00009DAB0122EA7F AS DateTime), NULL, N'POP3')
INSERT [WorkQueueExternalSourceType] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'75e3b51a-e4f5-48ed-941c-597f49fcc775', CAST(0x0000A2C700F231A0 AS DateTime), NULL, N'Sequential Workflow')
INSERT [WorkQueueExternalSourceType] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [Name]) VALUES (NULL, NULL, N'cb882379-80e7-41fd-bde7-c65045660ca7', CAST(0x0000A2C700F231A0 AS DateTime), NULL, N'Web Request')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WorkQueueCommandType](
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [WorkQueueCommandType] ([Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'Odstranění', NULL, NULL, N'8d4117e0-590b-4495-9fd3-de46d9e768af', CAST(0x00009DAB0122EA86 AS DateTime), NULL)
INSERT [WorkQueueCommandType] ([Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'Příkaz fronty', NULL, NULL, N'e7b1761f-7126-4a20-9da3-80872de80227', CAST(0x00009DAB0122EA87 AS DateTime), NULL)
INSERT [WorkQueueCommandType] ([Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'Přesun do fronty', NULL, NULL, N'cb9d374c-da12-46ff-8810-980597ae0a21', CAST(0x0000A17700DD6CD7 AS DateTime), NULL)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceType](
	[Name] [nvarchar](500) NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [ResourceType] ([Name], [RecordUpdated], [RecordCreatedServer], [Id], [RecordUpdatedServer], [RecordCreatedBy], [RecordCreated], [RecordUpdatedBy]) VALUES (N'Skupina', NULL, NULL, N'43e9fc8c-93f0-41d8-9fd1-f426fb870749', NULL, NULL, CAST(0x00009DAB0122EA8E AS DateTime), NULL)
INSERT [ResourceType] ([Name], [RecordUpdated], [RecordCreatedServer], [Id], [RecordUpdatedServer], [RecordCreatedBy], [RecordCreated], [RecordUpdatedBy]) VALUES (N'Člověk', NULL, NULL, N'e0360aa5-fa37-4e3d-9827-26f6b4a8905e', NULL, N'53c53bff-d458-4be4-94e3-34a94ba7b51d', CAST(0x0000989E01047485 AS DateTime), NULL)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WorkQueue](
	[refRemovalOrigamStateMachineEventTypeId] [uniqueidentifier] NULL,
	[RemovalNewValue] [nvarchar](200) NULL,
	[RemovalOldValue] [nvarchar](200) NULL,
	[ReferenceCode] [nvarchar](40) NOT NULL,
	[CreationNewValue] [nvarchar](200) NULL,
	[CreationFieldName] [nvarchar](200) NULL,
	[CreationOldValue] [nvarchar](200) NULL,
	[WorkQueueClass] [nvarchar](200) NOT NULL,
	[Roles] [nvarchar](500) NULL,
	[refCreationOrigamStateMachineEventTypeId] [uniqueidentifier] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[refWorkQueueExternalSourceTypeId] [uniqueidentifier] NULL,
	[ExternalSourceUserName] [nvarchar](50) NULL,
	[ExternalSourcePassword] [nvarchar](50) NULL,
	[ExternalSourceConnection] [nvarchar](200) NULL,
	[ExternalSourceLastMessage] [ntext] NULL,
	[ExternalSourceLastTime] [datetime] NULL,
	[ExternalSourceState] [ntext] NULL,
	[IsErrorQueue] [bit] NOT NULL,
	[RemovalFieldName] [nvarchar](200) NULL,
	[ReverseLookupFieldValues] [bit] NOT NULL,
	[CreationCondition] [ntext] NULL,
	[RemovalCondition] [ntext] NULL,
	[IsMessageCountDisplayed] [bit] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_ReferenceCode] ON [WorkQueue] 
(
	[ReferenceCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DimensionTransformationMatrix](
	[refSource4DimensionEntityId] [uniqueidentifier] NULL,
	[refSource2DimensionEntityId] [uniqueidentifier] NULL,
	[refTargetDimensionEntityId] [uniqueidentifier] NOT NULL,
	[refSource3DimensionEntityId] [uniqueidentifier] NULL,
	[refSource1DimensionEntityId] [uniqueidentifier] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refTargetDimensionEntityId] ON [DimensionTransformationMatrix] 
(
	[refTargetDimensionEntityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DimensionEntityRelation](
	[refSourceDimensionEntityId] [uniqueidentifier] NOT NULL,
	[refTargetDimensionEntityId] [uniqueidentifier] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Dimension4](
	[refDimensionTypeId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refDimensionTypeId] ON [Dimension4] 
(
	[refDimensionTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Dimension3](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[refDimensionTypeId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refDimensionTypeId] ON [Dimension3] 
(
	[refDimensionTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Dimension2](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[refDimensionTypeId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refDimensionTypeId] ON [Dimension2] 
(
	[refDimensionTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Dimension1](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[refDimensionTypeId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refDimensionTypeId] ON [Dimension1] 
(
	[refDimensionTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CounterDetail](
	[refCounterId] [uniqueidentifier] NOT NULL,
	[Prefix] [nvarchar](255) NULL,
	[ValidFrom] [datetime] NULL,
	[CurrentPosition] [int] NOT NULL,
	[Length] [int] NOT NULL,
	[Increment] [int] NOT NULL,
	[ValidTo] [datetime] NULL,
	[CounterFrom] [int] NOT NULL,
	[CounterTo] [int] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refCounterId] ON [CounterDetail] 
(
	[refCounterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamTraceWorkflowStep](
	[Category] [nvarchar](255) NOT NULL,
	[Subcategory] [nvarchar](255) NOT NULL,
	[Remark] [nvarchar](500) NULL,
	[WorkflowStepId] [uniqueidentifier] NOT NULL,
	[WorkflowStepPath] [ntext] NOT NULL,
	[Data1] [ntext] NULL,
	[Data2] [ntext] NULL,
	[refOrigamTraceWorkflowId] [uniqueidentifier] NOT NULL,
	[Message] [ntext] NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refOrigamTraceWorkflowId] ON [OrigamTraceWorkflowStep] 
(
	[refOrigamTraceWorkflowId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamCalendarDetail](
	[Date] [datetime] NOT NULL,
	[refOrigamCalendarId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refOrigamCalendarId] ON [OrigamCalendarDetail] 
(
	[refOrigamCalendarId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamCharacterTranslationDetail](
	[Target] [nvarchar](200) NOT NULL,
	[refOrigamCharacterTranslationId] [uniqueidentifier] NOT NULL,
	[Source] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[Priority] [int] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_OrigamCharacterTranslation] ON [OrigamCharacterTranslationDetail] 
(
	[refOrigamCharacterTranslationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamFormPanelConfig](
	[ProfileId] [uniqueidentifier] NOT NULL,
	[FormPanelId] [uniqueidentifier] NOT NULL,
	[WorkflowId] [uniqueidentifier] NULL,
	[refOrigamPanelFilterId] [uniqueidentifier] NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[DefaultView] [int] NOT NULL,
	[SettingsData] [ntext] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_Main] ON [OrigamFormPanelConfig] 
(
	[FormPanelId] ASC,
	[ProfileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamPanelFilterDetail](
	[StringValue] [nvarchar](300) NULL,
	[StringValue2] [nvarchar](300) NULL,
	[BoolValue2] [bit] NULL,
	[Operator] [int] NULL,
	[refOrigamPanelFilterId] [uniqueidentifier] NOT NULL,
	[ColumnName] [nvarchar](300) NOT NULL,
	[CurrencyValue2] [money] NULL,
	[GuidValue] [uniqueidentifier] NULL,
	[CurrencyValue] [money] NULL,
	[BoolValue] [bit] NULL,
	[GuidValue2] [uniqueidentifier] NULL,
	[DateValue] [datetime] NULL,
	[DateValue2] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refOrigamPanelFilterId] ON [OrigamPanelFilterDetail] 
(
	[refOrigamPanelFilterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamMapLayer](
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[refOrigamMapLayerTypeId] [uniqueidentifier] NOT NULL,
	[ReferenceCode] [nvarchar](20) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_Unique] ON [OrigamMapLayer] 
(
	[ReferenceCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamRounding](
	[refOrigamRoundingTypeId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Precision] [decimal](18, 10) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamRoleOrigamApplicationRole](
	[IsFormReadOnly] [bit] NOT NULL,
	[refOrigamRoleId] [uniqueidentifier] NOT NULL,
	[refOrigamApplicationRoleId] [uniqueidentifier] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_OrigamRole] ON [OrigamRoleOrigamApplicationRole] 
(
	[refOrigamRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_Unique] ON [OrigamRoleOrigamApplicationRole] 
(
	[refOrigamRoleId] ASC,
	[refOrigamApplicationRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamSyncConnection](
	[Name] [nvarchar](200) NOT NULL,
	[refOrigamSyncSystemId] [uniqueidentifier] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refOrigamSyncSystemId] ON [OrigamSyncConnection] 
(
	[refOrigamSyncSystemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamSyncProvider](
	[Name] [nvarchar](200) NOT NULL,
	[refOrigamSyncSystemId] [uniqueidentifier] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamTooltipHelpSubcontext](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[refOrigamTooltipHelpContextId] [uniqueidentifier] NOT NULL,
	[IdString] [nvarchar](500) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_OrigamTooltipHelpContext] ON [OrigamTooltipHelpSubcontext] 
(
	[refOrigamTooltipHelpContextId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
INSERT [OrigamTooltipHelpSubcontext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [refOrigamTooltipHelpContextId], [IdString], [Name]) VALUES (NULL, NULL, N'b645a64d-9aaf-4ee5-bc6e-e76947c0cf11', CAST(0x00009EBC00CCAC28 AS DateTime), NULL, N'9c453968-1dca-4b70-9474-1de935b2a343', N'Origam.controls.GridView', N'Tabulkové zobrazení')
INSERT [OrigamTooltipHelpSubcontext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [refOrigamTooltipHelpContextId], [IdString], [Name]) VALUES (NULL, NULL, N'98faa984-b9f3-455c-b10c-e3dbdf8119e9', CAST(0x00009EBC00CCAC29 AS DateTime), NULL, N'9c453968-1dca-4b70-9474-1de935b2a343', N'Origam.controls.FormView', N'Formulářové zobrazení')
INSERT [OrigamTooltipHelpSubcontext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [refOrigamTooltipHelpContextId], [IdString], [Name]) VALUES (NULL, NULL, N'ab31680e-2ca5-46c6-9a20-d5c2f7056952', CAST(0x00009EBC00CCAC29 AS DateTime), NULL, N'9c453968-1dca-4b70-9474-1de935b2a343', N'Origam.panels.GridPanel', N'Nezávislé na zobrazení')
INSERT [OrigamTooltipHelpSubcontext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [refOrigamTooltipHelpContextId], [IdString], [Name]) VALUES (NULL, NULL, N'2ef1ac43-7313-44e8-821b-0f8db0cd2ca7', CAST(0x00009EBC00CCAC29 AS DateTime), NULL, N'50446184-1abd-4bcf-89dd-1ce303213bf6', N'Origam.portal.PortalUIInfrastructure', N'Portál')
INSERT [OrigamTooltipHelpSubcontext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [refOrigamTooltipHelpContextId], [IdString], [Name]) VALUES (NULL, NULL, N'74305e49-3757-420c-899f-0620ea0d40bc', CAST(0x00009EBC00CCAC29 AS DateTime), NULL, N'fc797cd0-2aa9-49ca-a3a4-891ffff237f0', N'Origam.portal.tools.ToolsContainer', N'Nástroje')
INSERT [OrigamTooltipHelpSubcontext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [refOrigamTooltipHelpContextId], [IdString], [Name]) VALUES (NULL, NULL, N'628340bd-9197-4dff-9c26-0b261f15eabd', CAST(0x00009EBC00CCAC29 AS DateTime), NULL, N'13d7fa74-42a8-49b5-9ddf-caa887a39173', N'Origam.portal.tools.AuditPanel', N'Audit')
INSERT [OrigamTooltipHelpSubcontext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [refOrigamTooltipHelpContextId], [IdString], [Name]) VALUES (NULL, NULL, N'a988bf23-0e1d-43c1-9512-80694bebcb69', CAST(0x00009EBC00CCAC29 AS DateTime), NULL, N'b750974b-9999-40a8-bd7d-c8f976ec1e41', N'Origam.portal.tools.AttachmentsPanel', N'Přílohy')
INSERT [OrigamTooltipHelpSubcontext] ([RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated], [refOrigamTooltipHelpContextId], [IdString], [Name]) VALUES (NULL, NULL, N'3c73f616-f753-437d-9a8b-7b957d4d3316', CAST(0x00009EBC00CCAC29 AS DateTime), NULL, N'9c03e73f-aee4-4b57-8be4-fda555f83394', N'Origam.panels.TabPage', N'Záložka')
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamTooltipHelpContextComponent](
	[IdString] [nvarchar](500) NOT NULL,
	[refOrigamTooltipHelpSubcontextId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_OrigamTooltipHelpSubcontext] ON [OrigamTooltipHelpContextComponent] 
(
	[refOrigamTooltipHelpSubcontextId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
INSERT [OrigamTooltipHelpContextComponent] ([IdString], [refOrigamTooltipHelpSubcontextId], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'columnConfigButton', N'b645a64d-9aaf-4ee5-bc6e-e76947c0cf11', N'Tlačítko Konfigurace sloupců', NULL, NULL, N'9ca6d785-3e6b-4ae2-92e1-ad79efb06f80', CAST(0x00009EBC00CCAC29 AS DateTime), NULL)
INSERT [OrigamTooltipHelpContextComponent] ([IdString], [refOrigamTooltipHelpSubcontextId], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'addButton', N'ab31680e-2ca5-46c6-9a20-d5c2f7056952', N'Nový záznam', NULL, NULL, N'eb0b00ea-3424-4973-8eaa-c220fd7f6c0b', CAST(0x00009EBC00CCAC29 AS DateTime), NULL)
INSERT [OrigamTooltipHelpContextComponent] ([IdString], [refOrigamTooltipHelpSubcontextId], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'toolsSwitch', N'ab31680e-2ca5-46c6-9a20-d5c2f7056952', N'Info panel (přílohy, audit)', NULL, NULL, N'318fec55-22b5-4e71-85c5-12381281eb22', CAST(0x00009EBC00CCAC29 AS DateTime), NULL)
INSERT [OrigamTooltipHelpContextComponent] ([IdString], [refOrigamTooltipHelpSubcontextId], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'searchField', N'2ef1ac43-7313-44e8-821b-0f8db0cd2ca7', N'Hledání', NULL, NULL, N'7143e28b-fd10-428a-a978-3b26dd849f60', CAST(0x00009EBC00CCAC29 AS DateTime), NULL)
INSERT [OrigamTooltipHelpContextComponent] ([IdString], [refOrigamTooltipHelpSubcontextId], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'navigationControlsContainer', N'2ef1ac43-7313-44e8-821b-0f8db0cd2ca7', N'Okno navigace (menu)', NULL, NULL, N'bea4c11a-eb33-4a8a-8de0-e58185509964', CAST(0x00009EBC00CCAC29 AS DateTime), NULL)
INSERT [OrigamTooltipHelpContextComponent] ([IdString], [refOrigamTooltipHelpSubcontextId], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'tabPage', N'3c73f616-f753-437d-9a8b-7b957d4d3316', N'Záložka', NULL, NULL, N'3ebafd7a-5574-4526-bf61-7b3ad84023e4', CAST(0x00009EBC00CCAC29 AS DateTime), NULL)
INSERT [OrigamTooltipHelpContextComponent] ([IdString], [refOrigamTooltipHelpSubcontextId], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'auditPanel', N'74305e49-3757-420c-899f-0620ea0d40bc', N'Audit', NULL, NULL, N'7d710492-aa05-4162-96c8-b54f91ddd5ec', CAST(0x00009EBC00CCAC29 AS DateTime), NULL)
INSERT [OrigamTooltipHelpContextComponent] ([IdString], [refOrigamTooltipHelpSubcontextId], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'attachmentsPanel', N'74305e49-3757-420c-899f-0620ea0d40bc', N'Přílohy', NULL, NULL, N'b0864f66-06fa-49e1-82fd-259c0a76c9ea', CAST(0x00009EBC00CCAC29 AS DateTime), NULL)
INSERT [OrigamTooltipHelpContextComponent] ([IdString], [refOrigamTooltipHelpSubcontextId], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'actionButton', N'ab31680e-2ca5-46c6-9a20-d5c2f7056952', N'Akční tlačítko', NULL, NULL, N'2fff5132-9f85-4ae4-a1f0-48a607d82c46', CAST(0x00009EBC00CCAC29 AS DateTime), NULL)
INSERT [OrigamTooltipHelpContextComponent] ([IdString], [refOrigamTooltipHelpSubcontextId], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'saveButton', N'2ef1ac43-7313-44e8-821b-0f8db0cd2ca7', N'Uložit', NULL, NULL, N'13a85134-f924-481d-a7b6-3d76cd07f4d4', CAST(0x00009EBC00CCAC29 AS DateTime), NULL)
INSERT [OrigamTooltipHelpContextComponent] ([IdString], [refOrigamTooltipHelpSubcontextId], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'refreshButton', N'2ef1ac43-7313-44e8-821b-0f8db0cd2ca7', N'Obnovit', NULL, NULL, N'1a8cc728-7419-419e-afbf-9844f01de09b', CAST(0x00009EBC00CCAC29 AS DateTime), NULL)
INSERT [OrigamTooltipHelpContextComponent] ([IdString], [refOrigamTooltipHelpSubcontextId], [Name], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'generatedComponent', N'98faa984-b9f3-455c-b10c-e3dbdf8119e9', N'Prvek', NULL, NULL, N'013547a6-07cd-4aba-80ba-eb917ad6e0f6', CAST(0x00009EBC00CCAC29 AS DateTime), NULL)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamMapOrigamMapLayer](
	[DefaultEnabled] [bit] NOT NULL,
	[refOrigamMapId] [uniqueidentifier] NOT NULL,
	[refOrigamMapLayerId] [uniqueidentifier] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[SortOrder] [int] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_OrigamMap] ON [OrigamMapOrigamMapLayer] 
(
	[refOrigamMapId] ASC,
	[refOrigamMapLayerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_SortOrder] ON [OrigamMapOrigamMapLayer] 
(
	[refOrigamMapId] ASC,
	[SortOrder] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamMapLayerParameter](
	[refOrigamMapLayerId] [uniqueidentifier] NOT NULL,
	[Value] [ntext] NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_Unique] ON [OrigamMapLayerParameter] 
(
	[refOrigamMapLayerId] ASC,
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BusinessPartner](
	[FirstName] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NULL,
	[ReferenceCode] [nvarchar](40) NULL,
	[Name] [nvarchar](255) NOT NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[refDimension4Id] [uniqueidentifier] NULL,
	[refDimension3Id] [uniqueidentifier] NULL,
	[refDimension1Id] [uniqueidentifier] NULL,
	[refDimension2Id] [uniqueidentifier] NULL,
	[ParentId] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_ReferenceCode] ON [BusinessPartner] 
(
	[ReferenceCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
INSERT [BusinessPartner] ([FirstName], [UserName], [ReferenceCode], [Name], [RecordUpdated], [RecordCreatedServer], [Id], [RecordUpdatedServer], [RecordCreatedBy], [RecordCreated], [RecordUpdatedBy], [refDimension4Id], [refDimension3Id], [refDimension1Id], [refDimension2Id], [ParentId]) VALUES (N'Admin', N'UserName', N'ad', N'admin', NULL, NULL, N'a90bb6a1-4fb4-4fd8-b372-4a3610017e89', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WorkQueueCommand](
	[Param1] [nvarchar](200) NULL,
	[refWorkQueueId] [uniqueidentifier] NOT NULL,
	[refWorkQueueCommandTypeId] [uniqueidentifier] NOT NULL,
	[Text] [nvarchar](100) NOT NULL,
	[Param2] [nvarchar](200) NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[Command] [nvarchar](40) NULL,
	[AutoProcessingConditionXPath] [ntext] NULL,
	[refErrorWorkQueueId] [uniqueidentifier] NULL,
	[IsAutoProcessed] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_WorkQueue] ON [WorkQueueCommand] 
(
	[refWorkQueueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WorkQueueNotification](
	[refWorkQueueId] [uniqueidentifier] NOT NULL,
	[refOrigamNotificationTemplateId] [uniqueidentifier] NOT NULL,
	[refWorkQueueNotificationEventId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_WorkQueue] ON [WorkQueueNotification] 
(
	[refWorkQueueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WorkQueueNotificationContact](
	[refWorkQueueNotificationId] [uniqueidentifier] NOT NULL,
	[refOrigamNotificationChannelTypeId] [uniqueidentifier] NOT NULL,
	[Value] [nvarchar](200) NULL,
	[refWorkQueueNotificationContactTypeId] [uniqueidentifier] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[IsRecipient] [bit] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_WorkQueueNotification] ON [WorkQueueNotificationContact] 
(
	[refWorkQueueNotificationId] ASC,
	[IsRecipient] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WorkQueueEntry](
	[d8] [datetime] NULL,
	[g6] [uniqueidentifier] NULL,
	[i5] [int] NULL,
	[c2] [money] NULL,
	[s9] [nvarchar](200) NULL,
	[g5] [uniqueidentifier] NULL,
	[g14] [uniqueidentifier] NULL,
	[g15] [uniqueidentifier] NULL,
	[d2] [datetime] NULL,
	[c3] [money] NULL,
	[g20] [uniqueidentifier] NULL,
	[d9] [datetime] NULL,
	[g11] [uniqueidentifier] NULL,
	[g17] [uniqueidentifier] NULL,
	[g8] [uniqueidentifier] NULL,
	[g18] [uniqueidentifier] NULL,
	[s3] [nvarchar](200) NULL,
	[refRel1Id] [uniqueidentifier] NULL,
	[d6] [datetime] NULL,
	[d1] [datetime] NULL,
	[c6] [money] NULL,
	[refLockedByBusinessPartnerId] [uniqueidentifier] NULL,
	[refRel2Id] [uniqueidentifier] NULL,
	[refRel4Id] [uniqueidentifier] NULL,
	[f8] [decimal](18, 10) NULL,
	[f5] [decimal](18, 10) NULL,
	[d5] [datetime] NULL,
	[f9] [decimal](18, 10) NULL,
	[refRel7Id] [uniqueidentifier] NULL,
	[refRel5Id] [uniqueidentifier] NULL,
	[i2] [int] NULL,
	[refRel3Id] [uniqueidentifier] NULL,
	[m4] [ntext] NULL,
	[g10] [uniqueidentifier] NULL,
	[s2] [nvarchar](200) NULL,
	[c5] [money] NULL,
	[m3] [ntext] NULL,
	[i4] [int] NULL,
	[g3] [uniqueidentifier] NULL,
	[d10] [datetime] NULL,
	[g7] [uniqueidentifier] NULL,
	[f3] [decimal](18, 10) NULL,
	[i6] [int] NULL,
	[g2] [uniqueidentifier] NULL,
	[m5] [ntext] NULL,
	[m2] [ntext] NULL,
	[refId] [uniqueidentifier] NULL,
	[s5] [nvarchar](200) NULL,
	[d3] [datetime] NULL,
	[f7] [decimal](18, 10) NULL,
	[g12] [uniqueidentifier] NULL,
	[i8] [int] NULL,
	[c7] [money] NULL,
	[g9] [uniqueidentifier] NULL,
	[f10] [decimal](18, 10) NULL,
	[f6] [decimal](18, 10) NULL,
	[s4] [nvarchar](200) NULL,
	[d7] [datetime] NULL,
	[c4] [money] NULL,
	[s8] [nvarchar](200) NULL,
	[i7] [int] NULL,
	[c1] [money] NULL,
	[f2] [decimal](18, 10) NULL,
	[i3] [int] NULL,
	[c10] [money] NULL,
	[s7] [nvarchar](200) NULL,
	[s1] [nvarchar](200) NULL,
	[i9] [int] NULL,
	[i1] [int] NULL,
	[g4] [uniqueidentifier] NULL,
	[f4] [decimal](18, 10) NULL,
	[refWorkQueueId] [uniqueidentifier] NOT NULL,
	[m1] [ntext] NULL,
	[g19] [uniqueidentifier] NULL,
	[g13] [uniqueidentifier] NULL,
	[c9] [money] NULL,
	[refRel6Id] [uniqueidentifier] NULL,
	[f1] [decimal](18, 10) NULL,
	[s6] [nvarchar](200) NULL,
	[c8] [money] NULL,
	[g1] [uniqueidentifier] NULL,
	[s10] [nvarchar](200) NULL,
	[i10] [int] NULL,
	[IsLocked] [bit] NOT NULL,
	[g16] [uniqueidentifier] NULL,
	[d4] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[blob1] [image] NULL,
	[ErrorText] [ntext] NULL,
	[b1] [bit] NULL,
	[b2] [bit] NULL,
	[b3] [bit] NULL,
	[b4] [bit] NULL,
	[b5] [bit] NULL,
	[b6] [bit] NULL,
	[b7] [bit] NULL,
	[b8] [bit] NULL,
	[b9] [bit] NULL,
	[b10] [bit] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refId_WorkQueue] ON [WorkQueueEntry] 
(
	[refId] ASC,
	[refWorkQueueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_rel1] ON [WorkQueueEntry] 
(
	[refRel1Id] ASC,
	[refWorkQueueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_rel2] ON [WorkQueueEntry] 
(
	[refRel2Id] ASC,
	[refWorkQueueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_rel3] ON [WorkQueueEntry] 
(
	[refRel3Id] ASC,
	[refWorkQueueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_rel4] ON [WorkQueueEntry] 
(
	[refRel4Id] ASC,
	[refWorkQueueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_rel5] ON [WorkQueueEntry] 
(
	[refRel5Id] ASC,
	[refWorkQueueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_rel6] ON [WorkQueueEntry] 
(
	[refRel6Id] ASC,
	[refWorkQueueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_rel7] ON [WorkQueueEntry] 
(
	[refRel7Id] ASC,
	[refWorkQueueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_WorkQueue] ON [WorkQueueEntry] 
(
	[refWorkQueueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Organization](
	[Name] [nvarchar](200) NOT NULL,
	[ReferenceCode] [nvarchar](40) NOT NULL,
	[refBusinessPartnerId] [uniqueidentifier] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [Organization] ([Name], [ReferenceCode], [refBusinessPartnerId], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'Výchozí', N'001', NULL, NULL, NULL, N'28e22217-a876-44bd-bd6d-d4cab99bd434', CAST(0x00009CAD0042FE55 AS DateTime), NULL)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  VIEW [BusinessPartnerLookup]
AS
SELECT
	cast(BusinessPartner.Name + ISNULL(' ' + BusinessPartner.FirstName, '') + ISNULL(N', ' + ParentBusinessPartner.Name, N'') as nvarchar(1000)) AS LookupText,
	BusinessPartner.*
FROM
	dbo.BusinessPartner BusinessPartner 
	left outer join dbo.BusinessPartner ParentBusinessPartner
		ON ParentBusinessPartner.Id = BusinessPartner.ParentId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BusinessPartnerOrigamRole](
	[refBusinessPartnerId] [uniqueidentifier] NOT NULL,
	[refOrigamRoleId] [uniqueidentifier] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_Unique] ON [BusinessPartnerOrigamRole] 
(
	[refBusinessPartnerId] ASC,
	[refOrigamRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [refBusinessPartnerId] ON [BusinessPartnerOrigamRole] 
(
	[refBusinessPartnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
INSERT [BusinessPartnerOrigamRole] ([refBusinessPartnerId], [refOrigamRoleId], [RecordCreatedBy], [RecordUpdatedBy], [Id], [RecordCreated], [RecordUpdated]) VALUES (N'a90bb6a1-4fb4-4fd8-b372-4a3610017e89', N'57642d8a-c110-47a7-93d5-21fda8886cf1', N'a90bb6a1-4fb4-4fd8-b372-4a3610017e89', NULL, N'0c00fbb8-1cfc-4371-a907-ae40d1278cfa', CAST(0x0000A27C0092AD95 AS DateTime), NULL)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamFavoritesUserConfig](
	[refBusinessPartnerId] [uniqueidentifier] NOT NULL,
	[ConfigXml] [ntext] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_refBusinessPartnerId] ON [OrigamFavoritesUserConfig] 
(
	[refBusinessPartnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamTooltipHelp](
	[Name] [nvarchar](200) NOT NULL,
	[Text] [ntext] NOT NULL,
	[Priority] [int] NOT NULL,
	[ObjectId] [nvarchar](500) NULL,
	[refOrigamTooltipHelpPositionId] [uniqueidentifier] NOT NULL,
	[refOrigamTooltipHelpDestroyEventId] [uniqueidentifier] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[refOrigamTooltipHelpContextId] [uniqueidentifier] NOT NULL,
	[FormId] [uniqueidentifier] NOT NULL,
	[refOrigamTooltipHelpContextComponentId] [uniqueidentifier] NOT NULL,
	[refOrigamTooltipHelpSubcontextId] [uniqueidentifier] NOT NULL,
	[DestroyParameter] [nvarchar](200) NULL,
	[Roles] [nvarchar](200) NULL,
	[refOrigamTooltipHelpGroupId] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamTooltipHelpUsage](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[refOrigamTooltipHelpId] [uniqueidentifier] NOT NULL,
	[refBusinessPartnerId] [uniqueidentifier] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_OrigamTooltipHelp] ON [OrigamTooltipHelpUsage] 
(
	[refOrigamTooltipHelpId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamSyncProviderConfig](
	[refOrigamSyncProviderId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[refOrganizationId] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BusinessUnit](
	[Name] [nvarchar](200) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[refOrganizationId] [uniqueidentifier] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrganizationMapping](
	[refParentOrganizationId] [uniqueidentifier] NOT NULL,
	[refOrganizationId] [uniqueidentifier] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_Organization] ON [OrganizationMapping] 
(
	[refParentOrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Resource](
	[refBusinessUnitId] [uniqueidentifier] NULL,
	[refResourceTypeId] [uniqueidentifier] NULL,
	[Name] [nvarchar](255) NOT NULL,
	[refBusinessPartnerId] [uniqueidentifier] NOT NULL,
	[RecordUpdated] [datetime] NULL,
	[RecordCreatedServer] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordUpdatedServer] [datetime] NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[refOrganizationId] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [OrganizationMappingList] as
select 
	om.refParentOrganizationId,
	om.refOrganizationId
from 
	OrganizationMapping om

union

select 
	Organization.Id,
	Organization.Id
from 
	Organization
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamSyncConfig](
	[refSourceOrigamSyncConnectionId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[refSourceOrigamSyncProviderConfigId] [uniqueidentifier] NOT NULL,
	[refTargetOrigamSyncConnectionId] [uniqueidentifier] NOT NULL,
	[refTargetOrigamSyncProviderConfigId] [uniqueidentifier] NOT NULL,
	[refCounterReferenceCode] [nvarchar](40) NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrigamSyncRun](
	[refOrigamSyncConfigId] [uniqueidentifier] NOT NULL,
	[ReferenceCode] [nvarchar](40) NOT NULL,
	[TimeEnd] [datetime] NULL,
	[TimeStart] [datetime] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_refOrigamSyncConfigId] ON [OrigamSyncRun] 
(
	[refOrigamSyncConfigId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceAccessLevelResource](
	[refResourceAccessLevelId] [uniqueidentifier] NOT NULL,
	[refResourceId] [uniqueidentifier] NOT NULL,
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_Resource] ON [ResourceAccessLevelResource] 
(
	[refResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_ResourceAccessLevel] ON [ResourceAccessLevelResource] 
(
	[refResourceAccessLevelId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ResourceGroupMember](
	[RecordCreatedBy] [uniqueidentifier] NULL,
	[RecordUpdatedBy] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RecordCreated] [datetime] NULL,
	[RecordUpdated] [datetime] NULL,
	[refMemberResourceId] [uniqueidentifier] NOT NULL,
	[refGroupResourceId] [uniqueidentifier] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_MemberResource] ON [ResourceGroupMember] 
(
	[refMemberResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ix_Resource] ON [ResourceGroupMember] 
(
	[refMemberResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [ResourceList] AS
SELECT
	Id AS refGroupResourceId,
	Id AS refMemberResourceId
FROM 
	[Resource]
	
UNION ALL

SELECT
	refGroupResourceId,
	refMemberResourceId
FROM
	[ResourceGroupMember]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [ResourceAccessList] AS
SELECT
	ralr.refResourceAccessLevelId,
	rl.refMemberResourceId as refResourceId
FROM
	ResourceAccessLevelResource ralr
	INNER JOIN ResourceList rl ON rl.refGroupResourceId = ralr.refResourceId
GO
ALTER TABLE [Attachment] ADD  CONSTRAINT [DF_Attachment_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [WorkQueue]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueue_refCreationOrigamStateMachineEventTypeId_OrigamStateMachineEventType] FOREIGN KEY([refCreationOrigamStateMachineEventTypeId])
REFERENCES [OrigamStateMachineEventType] ([Id])
GO
ALTER TABLE [WorkQueue] CHECK CONSTRAINT [FK_WorkQueue_refCreationOrigamStateMachineEventTypeId_OrigamStateMachineEventType]
GO
ALTER TABLE [WorkQueue]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueue_refRemovalOrigamStateMachineEventTypeId_OrigamStateMachineEventType] FOREIGN KEY([refRemovalOrigamStateMachineEventTypeId])
REFERENCES [OrigamStateMachineEventType] ([Id])
GO
ALTER TABLE [WorkQueue] CHECK CONSTRAINT [FK_WorkQueue_refRemovalOrigamStateMachineEventTypeId_OrigamStateMachineEventType]
GO
ALTER TABLE [WorkQueue]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueue_refWorkQueueExternalSourceTypeId_WorkQueueExternalSourceType] FOREIGN KEY([refWorkQueueExternalSourceTypeId])
REFERENCES [WorkQueueExternalSourceType] ([Id])
GO
ALTER TABLE [WorkQueue] CHECK CONSTRAINT [FK_WorkQueue_refWorkQueueExternalSourceTypeId_WorkQueueExternalSourceType]
GO
ALTER TABLE [DimensionTransformationMatrix]  WITH CHECK ADD  CONSTRAINT [FK_DimensionTransformationMatrix_refSource1DimensionEntityId_DimensionEntity] FOREIGN KEY([refSource1DimensionEntityId])
REFERENCES [DimensionEntity] ([Id])
GO
ALTER TABLE [DimensionTransformationMatrix] CHECK CONSTRAINT [FK_DimensionTransformationMatrix_refSource1DimensionEntityId_DimensionEntity]
GO
ALTER TABLE [DimensionTransformationMatrix]  WITH CHECK ADD  CONSTRAINT [FK_DimensionTransformationMatrix_refSource2DimensionEntityId_DimensionEntity] FOREIGN KEY([refSource2DimensionEntityId])
REFERENCES [DimensionEntity] ([Id])
GO
ALTER TABLE [DimensionTransformationMatrix] CHECK CONSTRAINT [FK_DimensionTransformationMatrix_refSource2DimensionEntityId_DimensionEntity]
GO
ALTER TABLE [DimensionTransformationMatrix]  WITH CHECK ADD  CONSTRAINT [FK_DimensionTransformationMatrix_refSource3DimensionEntityId_DimensionEntity] FOREIGN KEY([refSource3DimensionEntityId])
REFERENCES [DimensionEntity] ([Id])
GO
ALTER TABLE [DimensionTransformationMatrix] CHECK CONSTRAINT [FK_DimensionTransformationMatrix_refSource3DimensionEntityId_DimensionEntity]
GO
ALTER TABLE [DimensionTransformationMatrix]  WITH CHECK ADD  CONSTRAINT [FK_DimensionTransformationMatrix_refSource4DimensionEntityId_DimensionEntity] FOREIGN KEY([refSource4DimensionEntityId])
REFERENCES [DimensionEntity] ([Id])
GO
ALTER TABLE [DimensionTransformationMatrix] CHECK CONSTRAINT [FK_DimensionTransformationMatrix_refSource4DimensionEntityId_DimensionEntity]
GO
ALTER TABLE [DimensionTransformationMatrix]  WITH CHECK ADD  CONSTRAINT [FK_DimensionTransformationMatrix_refTargetDimensionEntityId_DimensionEntity] FOREIGN KEY([refTargetDimensionEntityId])
REFERENCES [DimensionEntity] ([Id])
GO
ALTER TABLE [DimensionTransformationMatrix] CHECK CONSTRAINT [FK_DimensionTransformationMatrix_refTargetDimensionEntityId_DimensionEntity]
GO
ALTER TABLE [DimensionEntityRelation]  WITH CHECK ADD  CONSTRAINT [FK_DimensionEntityRelation_refSourceDimensionEntityId_DimensionEntity] FOREIGN KEY([refSourceDimensionEntityId])
REFERENCES [DimensionEntity] ([Id])
GO
ALTER TABLE [DimensionEntityRelation] CHECK CONSTRAINT [FK_DimensionEntityRelation_refSourceDimensionEntityId_DimensionEntity]
GO
ALTER TABLE [DimensionEntityRelation]  WITH CHECK ADD  CONSTRAINT [FK_DimensionEntityRelation_refTargetDimensionEntityId_DimensionEntity] FOREIGN KEY([refTargetDimensionEntityId])
REFERENCES [DimensionEntity] ([Id])
GO
ALTER TABLE [DimensionEntityRelation] CHECK CONSTRAINT [FK_DimensionEntityRelation_refTargetDimensionEntityId_DimensionEntity]
GO
ALTER TABLE [Dimension4]  WITH CHECK ADD  CONSTRAINT [FK_Dimension4_refDimensionTypeId_DimensionType] FOREIGN KEY([refDimensionTypeId])
REFERENCES [DimensionType] ([Id])
GO
ALTER TABLE [Dimension4] CHECK CONSTRAINT [FK_Dimension4_refDimensionTypeId_DimensionType]
GO
ALTER TABLE [Dimension3]  WITH CHECK ADD  CONSTRAINT [FK_Dimension3_refDimensionTypeId_DimensionType] FOREIGN KEY([refDimensionTypeId])
REFERENCES [DimensionType] ([Id])
GO
ALTER TABLE [Dimension3] CHECK CONSTRAINT [FK_Dimension3_refDimensionTypeId_DimensionType]
GO
ALTER TABLE [Dimension2]  WITH CHECK ADD  CONSTRAINT [FK_Dimension2_refDimensionTypeId_DimensionType] FOREIGN KEY([refDimensionTypeId])
REFERENCES [DimensionType] ([Id])
GO
ALTER TABLE [Dimension2] CHECK CONSTRAINT [FK_Dimension2_refDimensionTypeId_DimensionType]
GO
ALTER TABLE [Dimension1]  WITH CHECK ADD  CONSTRAINT [FK_Dimension1_refDimensionTypeId_DimensionType] FOREIGN KEY([refDimensionTypeId])
REFERENCES [DimensionType] ([Id])
GO
ALTER TABLE [Dimension1] CHECK CONSTRAINT [FK_Dimension1_refDimensionTypeId_DimensionType]
GO
ALTER TABLE [CounterDetail]  WITH CHECK ADD  CONSTRAINT [FK_CounterDetail_refCounterId_Counter] FOREIGN KEY([refCounterId])
REFERENCES [Counter] ([Id])
GO
ALTER TABLE [CounterDetail] CHECK CONSTRAINT [FK_CounterDetail_refCounterId_Counter]
GO
ALTER TABLE [OrigamTraceWorkflowStep]  WITH CHECK ADD  CONSTRAINT [FK_OrigamTraceWorkflowStep_refOrigamTraceWorkflowId_OrigamTraceWorkflow] FOREIGN KEY([refOrigamTraceWorkflowId])
REFERENCES [OrigamTraceWorkflow] ([Id])
GO
ALTER TABLE [OrigamTraceWorkflowStep] CHECK CONSTRAINT [FK_OrigamTraceWorkflowStep_refOrigamTraceWorkflowId_OrigamTraceWorkflow]
GO
ALTER TABLE [OrigamCalendarDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrigamCalendarDetail_refOrigamCalendarId_OrigamCalendar] FOREIGN KEY([refOrigamCalendarId])
REFERENCES [OrigamCalendar] ([Id])
GO
ALTER TABLE [OrigamCalendarDetail] CHECK CONSTRAINT [FK_OrigamCalendarDetail_refOrigamCalendarId_OrigamCalendar]
GO
ALTER TABLE [OrigamCharacterTranslationDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrigamCharacterTranslationDetail_refOrigamCharacterTranslationId_OrigamCharacterTranslation] FOREIGN KEY([refOrigamCharacterTranslationId])
REFERENCES [OrigamCharacterTranslation] ([Id])
GO
ALTER TABLE [OrigamCharacterTranslationDetail] CHECK CONSTRAINT [FK_OrigamCharacterTranslationDetail_refOrigamCharacterTranslationId_OrigamCharacterTranslation]
GO
ALTER TABLE [OrigamFormPanelConfig]  WITH CHECK ADD  CONSTRAINT [FK_OrigamFormPanelConfig_refOrigamPanelFilterId_OrigamPanelFilter] FOREIGN KEY([refOrigamPanelFilterId])
REFERENCES [OrigamPanelFilter] ([Id])
GO
ALTER TABLE [OrigamFormPanelConfig] CHECK CONSTRAINT [FK_OrigamFormPanelConfig_refOrigamPanelFilterId_OrigamPanelFilter]
GO
ALTER TABLE [OrigamPanelFilterDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrigamPanelFilterDetail_refOrigamPanelFilterId_OrigamPanelFilter] FOREIGN KEY([refOrigamPanelFilterId])
REFERENCES [OrigamPanelFilter] ([Id])
GO
ALTER TABLE [OrigamPanelFilterDetail] CHECK CONSTRAINT [FK_OrigamPanelFilterDetail_refOrigamPanelFilterId_OrigamPanelFilter]
GO
ALTER TABLE [OrigamMapLayer]  WITH CHECK ADD  CONSTRAINT [FK_OrigamMapLayer_refOrigamMapLayerTypeId_OrigamMapLayerType] FOREIGN KEY([refOrigamMapLayerTypeId])
REFERENCES [OrigamMapLayerType] ([Id])
GO
ALTER TABLE [OrigamMapLayer] CHECK CONSTRAINT [FK_OrigamMapLayer_refOrigamMapLayerTypeId_OrigamMapLayerType]
GO
ALTER TABLE [OrigamRounding]  WITH CHECK ADD  CONSTRAINT [FK_OrigamRounding_refOrigamRoundingTypeId_OrigamRoundingType] FOREIGN KEY([refOrigamRoundingTypeId])
REFERENCES [OrigamRoundingType] ([Id])
GO
ALTER TABLE [OrigamRounding] CHECK CONSTRAINT [FK_OrigamRounding_refOrigamRoundingTypeId_OrigamRoundingType]
GO
ALTER TABLE [OrigamRoleOrigamApplicationRole]  WITH CHECK ADD  CONSTRAINT [FK_OrigamRoleOrigamApplicationRole_refOrigamApplicationRoleId_OrigamApplicationRole] FOREIGN KEY([refOrigamApplicationRoleId])
REFERENCES [OrigamApplicationRole] ([Id])
GO
ALTER TABLE [OrigamRoleOrigamApplicationRole] CHECK CONSTRAINT [FK_OrigamRoleOrigamApplicationRole_refOrigamApplicationRoleId_OrigamApplicationRole]
GO
ALTER TABLE [OrigamRoleOrigamApplicationRole]  WITH CHECK ADD  CONSTRAINT [FK_OrigamRoleOrigamApplicationRole_refOrigamRoleId_OrigamRole] FOREIGN KEY([refOrigamRoleId])
REFERENCES [OrigamRole] ([Id])
GO
ALTER TABLE [OrigamRoleOrigamApplicationRole] CHECK CONSTRAINT [FK_OrigamRoleOrigamApplicationRole_refOrigamRoleId_OrigamRole]
GO
ALTER TABLE [OrigamSyncConnection]  WITH CHECK ADD  CONSTRAINT [FK_OrigamSyncConnection_refOrigamSyncSystemId_OrigamSyncSystem] FOREIGN KEY([refOrigamSyncSystemId])
REFERENCES [OrigamSyncSystem] ([Id])
GO
ALTER TABLE [OrigamSyncConnection] CHECK CONSTRAINT [FK_OrigamSyncConnection_refOrigamSyncSystemId_OrigamSyncSystem]
GO
ALTER TABLE [OrigamSyncProvider]  WITH CHECK ADD  CONSTRAINT [FK_OrigamSyncProvider_refOrigamSyncSystemId_OrigamSyncSystem] FOREIGN KEY([refOrigamSyncSystemId])
REFERENCES [OrigamSyncSystem] ([Id])
GO
ALTER TABLE [OrigamSyncProvider] CHECK CONSTRAINT [FK_OrigamSyncProvider_refOrigamSyncSystemId_OrigamSyncSystem]
GO
ALTER TABLE [OrigamTooltipHelpSubcontext]  WITH CHECK ADD  CONSTRAINT [FK_OrigamTooltipHelpSubcontext_refOrigamTooltipHelpContextId_OrigamTooltipHelpContext] FOREIGN KEY([refOrigamTooltipHelpContextId])
REFERENCES [OrigamTooltipHelpContext] ([Id])
GO
ALTER TABLE [OrigamTooltipHelpSubcontext] CHECK CONSTRAINT [FK_OrigamTooltipHelpSubcontext_refOrigamTooltipHelpContextId_OrigamTooltipHelpContext]
GO
ALTER TABLE [OrigamTooltipHelpContextComponent]  WITH CHECK ADD  CONSTRAINT [FK_OrigamTooltipHelpContextComponent_refOrigamTooltipHelpSubcontextId_OrigamTooltipHelpSubcontext] FOREIGN KEY([refOrigamTooltipHelpSubcontextId])
REFERENCES [OrigamTooltipHelpSubcontext] ([Id])
GO
ALTER TABLE [OrigamTooltipHelpContextComponent] CHECK CONSTRAINT [FK_OrigamTooltipHelpContextComponent_refOrigamTooltipHelpSubcontextId_OrigamTooltipHelpSubcontext]
GO
ALTER TABLE [OrigamMapOrigamMapLayer]  WITH CHECK ADD  CONSTRAINT [FK_OrigamMapOrigamMapLayer_refOrigamMapId_OrigamMap] FOREIGN KEY([refOrigamMapId])
REFERENCES [OrigamMap] ([Id])
GO
ALTER TABLE [OrigamMapOrigamMapLayer] CHECK CONSTRAINT [FK_OrigamMapOrigamMapLayer_refOrigamMapId_OrigamMap]
GO
ALTER TABLE [OrigamMapOrigamMapLayer]  WITH CHECK ADD  CONSTRAINT [FK_OrigamMapOrigamMapLayer_refOrigamMapLayerId_OrigamMapLayer] FOREIGN KEY([refOrigamMapLayerId])
REFERENCES [OrigamMapLayer] ([Id])
GO
ALTER TABLE [OrigamMapOrigamMapLayer] CHECK CONSTRAINT [FK_OrigamMapOrigamMapLayer_refOrigamMapLayerId_OrigamMapLayer]
GO
ALTER TABLE [OrigamMapLayerParameter]  WITH CHECK ADD  CONSTRAINT [FK_OrigamMapLayerParameter_refOrigamMapLayerId_OrigamMapLayer] FOREIGN KEY([refOrigamMapLayerId])
REFERENCES [OrigamMapLayer] ([Id])
GO
ALTER TABLE [OrigamMapLayerParameter] CHECK CONSTRAINT [FK_OrigamMapLayerParameter_refOrigamMapLayerId_OrigamMapLayer]
GO
ALTER TABLE [BusinessPartner]  WITH CHECK ADD  CONSTRAINT [FK_BusinessPartner_ParentId_BusinessPartner] FOREIGN KEY([ParentId])
REFERENCES [BusinessPartner] ([Id])
GO
ALTER TABLE [BusinessPartner] CHECK CONSTRAINT [FK_BusinessPartner_ParentId_BusinessPartner]
GO
ALTER TABLE [BusinessPartner]  WITH CHECK ADD  CONSTRAINT [FK_BusinessPartner_refDimension1Id_Dimension1] FOREIGN KEY([refDimension1Id])
REFERENCES [Dimension1] ([Id])
GO
ALTER TABLE [BusinessPartner] CHECK CONSTRAINT [FK_BusinessPartner_refDimension1Id_Dimension1]
GO
ALTER TABLE [BusinessPartner]  WITH CHECK ADD  CONSTRAINT [FK_BusinessPartner_refDimension2Id_Dimension2] FOREIGN KEY([refDimension2Id])
REFERENCES [Dimension2] ([Id])
GO
ALTER TABLE [BusinessPartner] CHECK CONSTRAINT [FK_BusinessPartner_refDimension2Id_Dimension2]
GO
ALTER TABLE [BusinessPartner]  WITH CHECK ADD  CONSTRAINT [FK_BusinessPartner_refDimension3Id_Dimension3] FOREIGN KEY([refDimension3Id])
REFERENCES [Dimension3] ([Id])
GO
ALTER TABLE [BusinessPartner] CHECK CONSTRAINT [FK_BusinessPartner_refDimension3Id_Dimension3]
GO
ALTER TABLE [BusinessPartner]  WITH CHECK ADD  CONSTRAINT [FK_BusinessPartner_refDimension4Id_Dimension4] FOREIGN KEY([refDimension4Id])
REFERENCES [Dimension4] ([Id])
GO
ALTER TABLE [BusinessPartner] CHECK CONSTRAINT [FK_BusinessPartner_refDimension4Id_Dimension4]
GO
ALTER TABLE [WorkQueueCommand]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueCommand_refErrorWorkQueueId_WorkQueue] FOREIGN KEY([refErrorWorkQueueId])
REFERENCES [WorkQueue] ([Id])
GO
ALTER TABLE [WorkQueueCommand] CHECK CONSTRAINT [FK_WorkQueueCommand_refErrorWorkQueueId_WorkQueue]
GO
ALTER TABLE [WorkQueueCommand]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueCommand_refWorkQueueCommandTypeId_WorkQueueCommandType] FOREIGN KEY([refWorkQueueCommandTypeId])
REFERENCES [WorkQueueCommandType] ([Id])
GO
ALTER TABLE [WorkQueueCommand] CHECK CONSTRAINT [FK_WorkQueueCommand_refWorkQueueCommandTypeId_WorkQueueCommandType]
GO
ALTER TABLE [WorkQueueCommand]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueCommand_refWorkQueueId_WorkQueue] FOREIGN KEY([refWorkQueueId])
REFERENCES [WorkQueue] ([Id])
GO
ALTER TABLE [WorkQueueCommand] CHECK CONSTRAINT [FK_WorkQueueCommand_refWorkQueueId_WorkQueue]
GO
ALTER TABLE [WorkQueueNotification]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueNotification_refOrigamNotificationTemplateId_OrigamNotificationTemplate] FOREIGN KEY([refOrigamNotificationTemplateId])
REFERENCES [OrigamNotificationTemplate] ([Id])
GO
ALTER TABLE [WorkQueueNotification] CHECK CONSTRAINT [FK_WorkQueueNotification_refOrigamNotificationTemplateId_OrigamNotificationTemplate]
GO
ALTER TABLE [WorkQueueNotification]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueNotification_refWorkQueueId_WorkQueue] FOREIGN KEY([refWorkQueueId])
REFERENCES [WorkQueue] ([Id])
GO
ALTER TABLE [WorkQueueNotification] CHECK CONSTRAINT [FK_WorkQueueNotification_refWorkQueueId_WorkQueue]
GO
ALTER TABLE [WorkQueueNotification]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueNotification_refWorkQueueNotificationEventId_WorkQueueNotificationEvent] FOREIGN KEY([refWorkQueueNotificationEventId])
REFERENCES [WorkQueueNotificationEvent] ([Id])
GO
ALTER TABLE [WorkQueueNotification] CHECK CONSTRAINT [FK_WorkQueueNotification_refWorkQueueNotificationEventId_WorkQueueNotificationEvent]
GO
ALTER TABLE [WorkQueueNotificationContact]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueNotificationContact_refOrigamNotificationChannelTypeId_OrigamNotificationChannelType] FOREIGN KEY([refOrigamNotificationChannelTypeId])
REFERENCES [OrigamNotificationChannelType] ([Id])
GO
ALTER TABLE [WorkQueueNotificationContact] CHECK CONSTRAINT [FK_WorkQueueNotificationContact_refOrigamNotificationChannelTypeId_OrigamNotificationChannelType]
GO
ALTER TABLE [WorkQueueNotificationContact]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueNotificationContact_refWorkQueueNotificationContactTypeId_WorkQueueNotificationContactType] FOREIGN KEY([refWorkQueueNotificationContactTypeId])
REFERENCES [WorkQueueNotificationContactType] ([Id])
GO
ALTER TABLE [WorkQueueNotificationContact] CHECK CONSTRAINT [FK_WorkQueueNotificationContact_refWorkQueueNotificationContactTypeId_WorkQueueNotificationContactType]
GO
ALTER TABLE [WorkQueueNotificationContact]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueNotificationContact_refWorkQueueNotificationId_WorkQueueNotification] FOREIGN KEY([refWorkQueueNotificationId])
REFERENCES [WorkQueueNotification] ([Id])
GO
ALTER TABLE [WorkQueueNotificationContact] CHECK CONSTRAINT [FK_WorkQueueNotificationContact_refWorkQueueNotificationId_WorkQueueNotification]
GO
ALTER TABLE [WorkQueueEntry]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueEntry_refLockedByBusinessPartnerId_BusinessPartner] FOREIGN KEY([refLockedByBusinessPartnerId])
REFERENCES [BusinessPartner] ([Id])
GO
ALTER TABLE [WorkQueueEntry] CHECK CONSTRAINT [FK_WorkQueueEntry_refLockedByBusinessPartnerId_BusinessPartner]
GO
ALTER TABLE [WorkQueueEntry]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueEntry_refWorkQueueId_WorkQueue] FOREIGN KEY([refWorkQueueId])
REFERENCES [WorkQueue] ([Id])
GO
ALTER TABLE [WorkQueueEntry] CHECK CONSTRAINT [FK_WorkQueueEntry_refWorkQueueId_WorkQueue]
GO
ALTER TABLE [Organization]  WITH CHECK ADD  CONSTRAINT [FK_Organization_refBusinessPartnerId_BusinessPartner] FOREIGN KEY([refBusinessPartnerId])
REFERENCES [BusinessPartner] ([Id])
GO
ALTER TABLE [Organization] CHECK CONSTRAINT [FK_Organization_refBusinessPartnerId_BusinessPartner]
GO
ALTER TABLE [BusinessPartnerOrigamRole]  WITH CHECK ADD  CONSTRAINT [FK_BusinessPartnerOrigamRole_refOrigamRoleId_OrigamRole] FOREIGN KEY([refOrigamRoleId])
REFERENCES [OrigamRole] ([Id])
GO
ALTER TABLE [BusinessPartnerOrigamRole] CHECK CONSTRAINT [FK_BusinessPartnerOrigamRole_refOrigamRoleId_OrigamRole]
GO
ALTER TABLE [BusinessPartnerOrigamRole]  WITH CHECK ADD  CONSTRAINT [FK_BusinessPartnerOrigamRole_refBusinessPartnerId_BusinessPartner] FOREIGN KEY([refBusinessPartnerId])
REFERENCES [BusinessPartner] ([Id])
GO
ALTER TABLE [BusinessPartnerOrigamRole] CHECK CONSTRAINT [FK_BusinessPartnerOrigamRole_refBusinessPartnerId_BusinessPartner]
GO
ALTER TABLE [OrigamFavoritesUserConfig]  WITH CHECK ADD  CONSTRAINT [FK_OrigamFavoritesUserConfig_refBusinessPartnerId_BusinessPartner] FOREIGN KEY([refBusinessPartnerId])
REFERENCES [BusinessPartner] ([Id])
GO
ALTER TABLE [OrigamFavoritesUserConfig] CHECK CONSTRAINT [FK_OrigamFavoritesUserConfig_refBusinessPartnerId_BusinessPartner]
GO
ALTER TABLE [OrigamTooltipHelp]  WITH CHECK ADD  CONSTRAINT [FK_OrigamTooltipHelp_refOrigamTooltipHelpContextComponentId_OrigamTooltipHelpContextComponent] FOREIGN KEY([refOrigamTooltipHelpContextComponentId])
REFERENCES [OrigamTooltipHelpContextComponent] ([Id])
GO
ALTER TABLE [OrigamTooltipHelp] CHECK CONSTRAINT [FK_OrigamTooltipHelp_refOrigamTooltipHelpContextComponentId_OrigamTooltipHelpContextComponent]
GO
ALTER TABLE [OrigamTooltipHelp]  WITH CHECK ADD  CONSTRAINT [FK_OrigamTooltipHelp_refOrigamTooltipHelpContextId_OrigamTooltipHelpContext] FOREIGN KEY([refOrigamTooltipHelpContextId])
REFERENCES [OrigamTooltipHelpContext] ([Id])
GO
ALTER TABLE [OrigamTooltipHelp] CHECK CONSTRAINT [FK_OrigamTooltipHelp_refOrigamTooltipHelpContextId_OrigamTooltipHelpContext]
GO
ALTER TABLE [OrigamTooltipHelp]  WITH CHECK ADD  CONSTRAINT [FK_OrigamTooltipHelp_refOrigamTooltipHelpDestroyEventId_OrigamTooltipHelpDestroyEvent] FOREIGN KEY([refOrigamTooltipHelpDestroyEventId])
REFERENCES [OrigamTooltipHelpDestroyEvent] ([Id])
GO
ALTER TABLE [OrigamTooltipHelp] CHECK CONSTRAINT [FK_OrigamTooltipHelp_refOrigamTooltipHelpDestroyEventId_OrigamTooltipHelpDestroyEvent]
GO
ALTER TABLE [OrigamTooltipHelp]  WITH CHECK ADD  CONSTRAINT [FK_OrigamTooltipHelp_refOrigamTooltipHelpGroupId_OrigamTooltipHelpGroup] FOREIGN KEY([refOrigamTooltipHelpGroupId])
REFERENCES [OrigamTooltipHelpGroup] ([Id])
GO
ALTER TABLE [OrigamTooltipHelp] CHECK CONSTRAINT [FK_OrigamTooltipHelp_refOrigamTooltipHelpGroupId_OrigamTooltipHelpGroup]
GO
ALTER TABLE [OrigamTooltipHelp]  WITH CHECK ADD  CONSTRAINT [FK_OrigamTooltipHelp_refOrigamTooltipHelpPositionId_OrigamTooltipHelpPosition] FOREIGN KEY([refOrigamTooltipHelpPositionId])
REFERENCES [OrigamTooltipHelpPosition] ([Id])
GO
ALTER TABLE [OrigamTooltipHelp] CHECK CONSTRAINT [FK_OrigamTooltipHelp_refOrigamTooltipHelpPositionId_OrigamTooltipHelpPosition]
GO
ALTER TABLE [OrigamTooltipHelp]  WITH CHECK ADD  CONSTRAINT [FK_OrigamTooltipHelp_refOrigamTooltipHelpSubcontextId_OrigamTooltipHelpSubcontext] FOREIGN KEY([refOrigamTooltipHelpSubcontextId])
REFERENCES [OrigamTooltipHelpSubcontext] ([Id])
GO
ALTER TABLE [OrigamTooltipHelp] CHECK CONSTRAINT [FK_OrigamTooltipHelp_refOrigamTooltipHelpSubcontextId_OrigamTooltipHelpSubcontext]
GO
ALTER TABLE [OrigamTooltipHelpUsage]  WITH CHECK ADD  CONSTRAINT [FK_OrigamTooltipHelpUsage_refOrigamTooltipHelpId_OrigamTooltipHelp] FOREIGN KEY([refOrigamTooltipHelpId])
REFERENCES [OrigamTooltipHelp] ([Id])
GO
ALTER TABLE [OrigamTooltipHelpUsage] CHECK CONSTRAINT [FK_OrigamTooltipHelpUsage_refOrigamTooltipHelpId_OrigamTooltipHelp]
GO
ALTER TABLE [OrigamTooltipHelpUsage]  WITH CHECK ADD  CONSTRAINT [FK_OrigamTooltipHelpUsage_refBusinessPartnerId_BusinessPartner] FOREIGN KEY([refBusinessPartnerId])
REFERENCES [BusinessPartner] ([Id])
GO
ALTER TABLE [OrigamTooltipHelpUsage] CHECK CONSTRAINT [FK_OrigamTooltipHelpUsage_refBusinessPartnerId_BusinessPartner]
GO
ALTER TABLE [OrigamSyncProviderConfig]  WITH CHECK ADD  CONSTRAINT [FK_OrigamSyncProviderConfig_refOrigamSyncProviderId_OrigamSyncProvider] FOREIGN KEY([refOrigamSyncProviderId])
REFERENCES [OrigamSyncProvider] ([Id])
GO
ALTER TABLE [OrigamSyncProviderConfig] CHECK CONSTRAINT [FK_OrigamSyncProviderConfig_refOrigamSyncProviderId_OrigamSyncProvider]
GO
ALTER TABLE [OrigamSyncProviderConfig]  WITH CHECK ADD  CONSTRAINT [FK_OrigamSyncProviderConfig_refOrganizationId_Organization] FOREIGN KEY([refOrganizationId])
REFERENCES [Organization] ([Id])
GO
ALTER TABLE [OrigamSyncProviderConfig] CHECK CONSTRAINT [FK_OrigamSyncProviderConfig_refOrganizationId_Organization]
GO
ALTER TABLE [BusinessUnit]  WITH CHECK ADD  CONSTRAINT [FK_BusinessUnit_refOrganizationId_Organization] FOREIGN KEY([refOrganizationId])
REFERENCES [Organization] ([Id])
GO
ALTER TABLE [BusinessUnit] CHECK CONSTRAINT [FK_BusinessUnit_refOrganizationId_Organization]
GO
ALTER TABLE [OrganizationMapping]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationMapping_refOrganizationId_Organization] FOREIGN KEY([refOrganizationId])
REFERENCES [Organization] ([Id])
GO
ALTER TABLE [OrganizationMapping] CHECK CONSTRAINT [FK_OrganizationMapping_refOrganizationId_Organization]
GO
ALTER TABLE [OrganizationMapping]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationMapping_refParentOrganizationId_Organization] FOREIGN KEY([refParentOrganizationId])
REFERENCES [Organization] ([Id])
GO
ALTER TABLE [OrganizationMapping] CHECK CONSTRAINT [FK_OrganizationMapping_refParentOrganizationId_Organization]
GO
ALTER TABLE [Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_refBusinessPartnerId_BusinessPartner] FOREIGN KEY([refBusinessPartnerId])
REFERENCES [BusinessPartner] ([Id])
GO
ALTER TABLE [Resource] CHECK CONSTRAINT [FK_Resource_refBusinessPartnerId_BusinessPartner]
GO
ALTER TABLE [Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_refBusinessUnitId_BusinessUnit] FOREIGN KEY([refBusinessUnitId])
REFERENCES [BusinessUnit] ([Id])
GO
ALTER TABLE [Resource] CHECK CONSTRAINT [FK_Resource_refBusinessUnitId_BusinessUnit]
GO
ALTER TABLE [Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_refOrganizationId_Organization] FOREIGN KEY([refOrganizationId])
REFERENCES [Organization] ([Id])
GO
ALTER TABLE [Resource] CHECK CONSTRAINT [FK_Resource_refOrganizationId_Organization]
GO
ALTER TABLE [Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_refResourceTypeId_ResourceType] FOREIGN KEY([refResourceTypeId])
REFERENCES [ResourceType] ([Id])
GO
ALTER TABLE [Resource] CHECK CONSTRAINT [FK_Resource_refResourceTypeId_ResourceType]
GO
ALTER TABLE [OrigamSyncConfig]  WITH CHECK ADD  CONSTRAINT [FK_OrigamSyncConfig_refCounterReferenceCode_Counter] FOREIGN KEY([refCounterReferenceCode])
REFERENCES [Counter] ([ReferenceCode])
GO
ALTER TABLE [OrigamSyncConfig] CHECK CONSTRAINT [FK_OrigamSyncConfig_refCounterReferenceCode_Counter]
GO
ALTER TABLE [OrigamSyncConfig]  WITH CHECK ADD  CONSTRAINT [FK_OrigamSyncConfig_refSourceOrigamSyncConnectionId_OrigamSyncConnection] FOREIGN KEY([refSourceOrigamSyncConnectionId])
REFERENCES [OrigamSyncConnection] ([Id])
GO
ALTER TABLE [OrigamSyncConfig] CHECK CONSTRAINT [FK_OrigamSyncConfig_refSourceOrigamSyncConnectionId_OrigamSyncConnection]
GO
ALTER TABLE [OrigamSyncConfig]  WITH CHECK ADD  CONSTRAINT [FK_OrigamSyncConfig_refSourceOrigamSyncProviderConfigId_OrigamSyncProviderConfig] FOREIGN KEY([refSourceOrigamSyncProviderConfigId])
REFERENCES [OrigamSyncProviderConfig] ([Id])
GO
ALTER TABLE [OrigamSyncConfig] CHECK CONSTRAINT [FK_OrigamSyncConfig_refSourceOrigamSyncProviderConfigId_OrigamSyncProviderConfig]
GO
ALTER TABLE [OrigamSyncConfig]  WITH CHECK ADD  CONSTRAINT [FK_OrigamSyncConfig_refTargetOrigamSyncConnectionId_OrigamSyncConnection] FOREIGN KEY([refTargetOrigamSyncConnectionId])
REFERENCES [OrigamSyncConnection] ([Id])
GO
ALTER TABLE [OrigamSyncConfig] CHECK CONSTRAINT [FK_OrigamSyncConfig_refTargetOrigamSyncConnectionId_OrigamSyncConnection]
GO
ALTER TABLE [OrigamSyncConfig]  WITH CHECK ADD  CONSTRAINT [FK_OrigamSyncConfig_refTargetOrigamSyncProviderConfigId_OrigamSyncProviderConfig] FOREIGN KEY([refTargetOrigamSyncProviderConfigId])
REFERENCES [OrigamSyncProviderConfig] ([Id])
GO
ALTER TABLE [OrigamSyncConfig] CHECK CONSTRAINT [FK_OrigamSyncConfig_refTargetOrigamSyncProviderConfigId_OrigamSyncProviderConfig]
GO
ALTER TABLE [OrigamSyncRun]  WITH CHECK ADD  CONSTRAINT [FK_OrigamSyncRun_refOrigamSyncConfigId_OrigamSyncConfig] FOREIGN KEY([refOrigamSyncConfigId])
REFERENCES [OrigamSyncConfig] ([Id])
GO
ALTER TABLE [OrigamSyncRun] CHECK CONSTRAINT [FK_OrigamSyncRun_refOrigamSyncConfigId_OrigamSyncConfig]
GO
ALTER TABLE [ResourceAccessLevelResource]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAccessLevelResource_refResourceAccessLevelId_ResourceAccessLevel] FOREIGN KEY([refResourceAccessLevelId])
REFERENCES [ResourceAccessLevel] ([Id])
GO
ALTER TABLE [ResourceAccessLevelResource] CHECK CONSTRAINT [FK_ResourceAccessLevelResource_refResourceAccessLevelId_ResourceAccessLevel]
GO
ALTER TABLE [ResourceAccessLevelResource]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAccessLevelResource_refResourceId_Resource] FOREIGN KEY([refResourceId])
REFERENCES [Resource] ([Id])
GO
ALTER TABLE [ResourceAccessLevelResource] CHECK CONSTRAINT [FK_ResourceAccessLevelResource_refResourceId_Resource]
GO
ALTER TABLE [ResourceGroupMember]  WITH CHECK ADD  CONSTRAINT [FK_ResourceGroupMember_refGroupResourceId_Resource] FOREIGN KEY([refGroupResourceId])
REFERENCES [Resource] ([Id])
GO
ALTER TABLE [ResourceGroupMember] CHECK CONSTRAINT [FK_ResourceGroupMember_refGroupResourceId_Resource]
GO
ALTER TABLE [ResourceGroupMember]  WITH CHECK ADD  CONSTRAINT [FK_ResourceGroupMember_refMemberResourceId_Resource] FOREIGN KEY([refMemberResourceId])
REFERENCES [Resource] ([Id])
GO
ALTER TABLE [ResourceGroupMember] CHECK CONSTRAINT [FK_ResourceGroupMember_refMemberResourceId_Resource]
GO