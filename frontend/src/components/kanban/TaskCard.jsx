import React from "react";
import { Draggable } from "@hello-pangea/dnd";

const TaskCard = ({ issue, index, onClick }) => {
  return (
    <Draggable draggableId={issue.id.toString()} index={index}>
      {(provided) => (
        <div
          onClick={onClick}
          ref={provided.innerRef}
          {...provided.draggableProps}
          {...provided.dragHandleProps}
          className="bg-white rounded-xl p-4 shadow-sm border border-gray-200 cursor-pointer hover:shadow-md transition-shadow"
        >
          <h4 className="text-base font-semibold text-gray-800 truncate">
            {issue.title}
          </h4>
          <p className="text-sm text-gray-600 mt-1 line-clamp-3">
            {issue.description}
          </p>
        </div>
      )}
    </Draggable>
  );
};

export default TaskCard;