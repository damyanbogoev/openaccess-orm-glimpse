namespace OpenAccess.Glimpse
{
    public static class Constants
    {
        // Listener
        public const string ListenerName = "OpenAccess ORM Glimpse Trace Listener";

        // OAGlimpseTab
        public const string ProductName = "OpenAccess ORM";
        public const string DocumentationUri = @"http://documentation.telerik.com/openaccess-orm/documentation/feature-reference/tools/model-settings-dialog/backend-settings/feature-ref-tools-visual-designer-model-settings-backend-trace-logging";
        public const string MetricsKey = "Metric";
        public const string MetricsValue = "Value";
        public const string MetricsTimestamp = "Timestamp";
        public const string MetricsDuration = "Duration";

        // Second Level Cache Metrics
        public const string EvictAll = "cache.evict.all";
        public const string EvictObjects = "cache.evict.oids";
        public const string EvictClasses = "cache.evict.classes";
        public const string EvictAllCaption = "Second Level Cache Evict All";
        public const string EvictObjectsCaption = "Second Level Cache Evict Objects";
        public const string EvictClassesCaption = "Second Level Cache Evict Types";

        // Transaction Metrics
        public const string TransactionBegin = "sm.begin";
        public const string TransationCommit = "sm.commit";
        public const string TransactionRollback = "sm.rollback";
        public const string TransactionEnlist = "sm.managed.start";
        public const string TransactionDelist = "sm.managed.finished";
        public const string TransactionBeginCaption = "Transaction Begin";
        public const string TransactionCommitCaption = "Transaction Commit";
        public const string TransactionRollbackCaption = "Transaction Rollback";
        public const string TransactionEnlistCaption = "Transaction Enlist";
        public const string TransactionDelistCaption = "Transaction Delist";

        // Connection Metrics
        public const string ConnectionOpen = "driver.con.connect";
        public const string ConnectionClose = "driver.con.close";
        public const string ConnectionTransactionCommit = "driver.con.commit";
        public const string ConnectionTransactionRollback = "driver.con.rollback";
        public const string ConnectionOpenCaption = "Connection Open";
        public const string ConnectionCloseCaption = "Connection Close";

        // Command Metrics
        public const string CommandQuery = "driver.stat.execQuery";
        public const string CommandUpdate = "driver.stat.execUpdate";
        public const string CommandExecuteScalar = "driver.stat.executeScalar";
        public const string CommandExecuteNonQuery = "driver.stat.executeNonQuery";
        public const string CloseReader = "driver.rs.close";
        public const string AdoCommandBatchExecute = "driver.stat.execBatch";
        public const string CommandQueryCaption = "Command Query";
        public const string CommandUpdateCaption = "Command Update";
        public const string CommandExecuteScalarCaption = "Command ExecuteScalar";
        public const string CommandExecuteNonQueryCaption = "Command ExecuteNonQuery";
        public const string AdoCommandBatchExecuteCaption = "ADO Batch Command";

        public const string SqlQueryExecuted = "SQL Query: Executed";
    }
}
