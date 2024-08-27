import express from 'express';
import compression from 'compression';
import cors from 'cors';
import { swaggerSetup } from '@utils/swagger';
import { notFoundHandler } from '@utils/default-routers';
import prisma from 'client';
import httpStatus from 'http-status';

const app = express();

if (process.env.NODE_ENV !== 'production') {

}

app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(compression());
app.use(cors());
app.options("*", cors());
app.use('/api-docs', swaggerSetup);
app.get('/api/v1/users',
    async (req, res) => {
        var users = await prisma.user.findMany()
        res.json(
            {
                status: httpStatus.OK,
                data: users,
            }
        )
    }
)

app.use(notFoundHandler);

export default app;
