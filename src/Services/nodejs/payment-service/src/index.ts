import { config } from "@infrastructure/config";
import app from "./app";
import prisma from "./client";
import logger from "@infrastructure/logger/logger";

function createServer() {
    return prisma.$connect().then(
        () => {
            logger.info("Connected to database");
            return app.listen(config.PORT, () => {
                logger.info(`Server running on port ${config.PORT}`);
            })
        }
    ).catch(
        (error: any) => {
            console.error(error);
            return undefined;
        }
    )
};

async function main() {
    const server = await createServer();
    if (!server)
        return;

    function exitHandler() {
        server?.close(() => {
            logger.info("Server stopped");
            process.exit(1);
        });
    }

    function unExpectedErrorHandler(error: Error) {
        logger.error(error);
        exitHandler();
    }

    process.on('uncaughtException', unExpectedErrorHandler);
    process.on('unhandledRejection', unExpectedErrorHandler);
    process.on('SIGTERM',
        () => {
            logger.info('SIGTERM received');
            exitHandler();
        }
    );
}

main();
