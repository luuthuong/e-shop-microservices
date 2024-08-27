import { RequestHandler } from "express";
import httpStatus from "http-status";

export const notFoundHandler: RequestHandler = (req, res) => {
    res.status(httpStatus.NOT_FOUND).json(
        {
            status: httpStatus.NOT_FOUND,
            message: 'Not Found'
        }
    )
};
